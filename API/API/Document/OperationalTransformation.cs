using API.Document.Domain;

namespace API.Document;

public static class OperationalTransformation
{
    public static DocumentAction Transform(DocumentAction action, List<DocumentAction> completedActions)
    {
        var docVersion = completedActions.Count;
        if (action.Revision == docVersion + 1) return action;
        if (action.Revision > docVersion + 1) throw new Exception("Invalid revision number");

        var transformedAction = completedActions[action.Revision..].Aggregate(action, TransformAction);
        if (transformedAction == null) throw new Exception("Invalid transformation");
        return transformedAction;
    }

    public static DocumentAction TransformAction(DocumentAction newAction, DocumentAction oldAction)
    {
        if (newAction.IsInsert() && oldAction.IsInsert()) return InsertOverInsert(newAction, oldAction);
        if (newAction.IsInsert() && oldAction.IsDelete()) return InsertOverDelete(newAction, oldAction);
        if (newAction.IsInsert() && oldAction.IsUpdate()) return InsertOverUpdate(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsInsert()) return DeleteOverInsert(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsDelete()) return DeleteOverDelete(newAction, oldAction);
        if (newAction.IsDelete() && oldAction.IsUpdate()) return DeleteOverUpdate(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsInsert()) return UpdateOverInsert(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsDelete()) return UpdateOverDelete(newAction, oldAction);
        if (newAction.IsUpdate() && oldAction.IsUpdate()) return UpdateOverUpdate(newAction, oldAction);
        throw new Exception("Unhandled condition. This should never happen.");
    }
    
    public static DocumentAction InsertOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        if (newAction.Position < oldAction.Position) return new DocumentAction
        {
            Client = newAction.Client,
            Insert = newAction.Insert,
            Position = newAction.Position, 
            Revision = oldAction.Revision + 1,
        };
        
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var newPosition = newAction.Position + oldInsertLength;
        return new DocumentAction
        {
            Client = newAction.Client,
            Insert = newAction.Insert,
            Position = newPosition,
            Revision = oldAction.Revision + 1,
        };
    }
    
    public static DocumentAction DeleteOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var deleteLength = newAction.Delete ?? 0;
        var insertLength = oldAction.Insert?.Length ?? 0;
            
        if (newAction.Position >= oldAction.Position)
        {

            var newPosition = newAction.Position + insertLength; 
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = deleteLength,
                Position = newPosition,
                Revision = oldAction.Revision + 1,
            };
        }

        var positionDelta = oldAction.Position - newAction.Position;
        if (positionDelta >= deleteLength) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = deleteLength,
            Position = newAction.Position,
            Revision = oldAction.Revision + 1,
        };
        
        var newDelete = deleteLength + insertLength;
        return new DocumentAction
        {
            Client = newAction.Client,
            Insert = oldAction.Insert,
            Delete = newDelete,
            Position = newAction.Position, 
            Revision = oldAction.Revision + 1,
        };
    }
    
    public static DocumentAction UpdateOverInsert(DocumentAction newAction, DocumentAction oldAction)
    {
        var deleteLength = newAction.Delete ?? 0;
        var newInsertLength = newAction.Insert?.Length ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        
        if (newAction.Position >= oldAction.Position)
        {
            var newPosition = newAction.Position + oldInsertLength;
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = deleteLength,
                Insert = newAction.Insert,
                Position = newPosition,
                Revision = oldAction.Revision + 1,
            };
        }
        
        var positionDelta = oldAction.Position - newAction.Position;
        if (positionDelta >= deleteLength) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = deleteLength,
            Insert = newAction.Insert,
            Position = newAction.Position,
            Revision = oldAction.Revision + 1,
        };

        var newDelete = deleteLength + newInsertLength;
        var newInsert = (newAction.Insert ?? "") + (oldAction.Insert ?? ""); 
        return new DocumentAction
        {
            Client = newAction.Client,
            Delete = newDelete,
            Insert = newInsert,
            Position = newAction.Position,
            Revision = oldAction.Revision + 1,
        };
    }
    
    public static DocumentAction InsertOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var deleteLength = oldAction.Delete ?? 0;
        if (newAction.Position < oldAction.Position) return new DocumentAction
        {
            Client = newAction.Client,
            Insert = newAction.Insert,
            Position = newAction.Position,
            Revision = oldAction.Revision + 1,
        };

        var positionDelta = newAction.Position - oldAction.Position;
        var delta = positionDelta > deleteLength ? deleteLength : positionDelta;
        var newPosition = newAction.Position - delta;
        return new DocumentAction
        {
            Client = newAction.Client,
            Insert = newAction.Insert,
            Position = newPosition,
            Revision = oldAction.Revision + 1,
        };
    }
    
    public static DocumentAction DeleteOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;

        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
            {
                return new DocumentAction
                {
                    Client = newAction.Client,
                    Delete = newAction.Delete,
                    Position = newAction.Position,
                    Revision = oldAction.Revision + 1,
                };
            }

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = positionDelta + deleteRemainingOrZero,
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = newDelete,
            Position = newAction.Position - oldDelete,
            Revision = oldAction.Revision + 1,
        };
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        if (deleteSurplus <= 0)
        {
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = 0, 
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        return new DocumentAction
        {
            Client = newAction.Client,
            Delete = deleteSurplus,
            Position = oldAction.Position,
            Revision = oldAction.Revision + 1,
        };
    }

    public static DocumentAction UpdateOverDelete(DocumentAction newAction, DocumentAction oldAction)
    {
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
            {
                return new DocumentAction
                {
                    Client = newAction.Client,
                    Delete = newAction.Delete,
                    Insert = newAction.Insert,
                    Position = newAction.Position,
                    Revision = oldAction.Revision + 1,
                };
            }

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = positionDelta + deleteRemainingOrZero,
                Insert = newAction.Insert,
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = newDelete,
            Insert = newAction.Insert,
            Position = newAction.Position - oldDelete,
            Revision = oldAction.Revision + 1,
        };
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        if (deleteSurplus <= 0)
        {
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = 0, 
                Insert = newAction.Insert,
                Position = newAction.Position - positionDif,
                Revision = oldAction.Revision + 1,
            };
        }
        
        return new DocumentAction
        {
            Client = newAction.Client,
            Delete = deleteSurplus,
            Insert = newAction.Insert,
            Position = newAction.Position - positionDif,
            Revision = oldAction.Revision + 1,
        };
    }

    public static DocumentAction InsertOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {   
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        if (newAction.Position < oldAction.Position)
        {
            return new DocumentAction
            {
                Client = newAction.Client,
                Insert = newAction.Insert,
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }

        var positionDelta = newAction.Position - oldAction.Position;
        if (positionDelta > oldDelete)
        {
            return new DocumentAction
            {
                Client = newAction.Client,
                Insert = newAction.Insert,
                Position = newAction.Position + oldNetLength,
                Revision = oldAction.Revision + 1,
            };
        }
        
        return new DocumentAction
        {
            Client = newAction.Client,
            Insert = newAction.Insert,
            Position = newAction.Position + oldInsertLength - positionDelta,
            Revision = oldAction.Revision + 1,
        };
    }

    public static DocumentAction DeleteOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
            {
                return new DocumentAction
                {
                    Client = newAction.Client,
                    Delete = newAction.Delete,
                    Position = newAction.Position,
                    Revision = oldAction.Revision + 1,
                };
            }

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = positionDelta + deleteRemainingOrZero + oldInsertLength,
                Insert = oldAction.Insert,
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = newDelete,
            Position = newAction.Position + oldNetLength,
            Revision = oldAction.Revision + 1,
        };
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        if (deleteSurplus <= 0)
        {
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = 0, 
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        return new DocumentAction
        {
            Client = newAction.Client,
            Delete = oldInsertLength - positionDif + deleteSurplus,
            Insert = oldAction.Insert?[positionDif..] ?? "",
            Position = newAction.Position, 
            Revision = oldAction.Revision + 1,
        };
    }

    public static DocumentAction UpdateOverUpdate(DocumentAction newAction, DocumentAction oldAction)
    {
        var newDelete = newAction.Delete ?? 0;
        var oldDelete = oldAction.Delete ?? 0;
        var oldInsertLength = oldAction.Insert?.Length ?? 0;
        var oldNetLength = oldInsertLength - oldDelete;
        
        if (newAction.Position < oldAction.Position)
        {
            var positionDelta = oldAction.Position - newAction.Position;
            if (positionDelta > newDelete)
            {
                return new DocumentAction
                {
                    Client = newAction.Client,
                    Delete = newAction.Delete,
                    Insert = newAction.Insert,
                    Position = newAction.Position,
                    Revision = oldAction.Revision + 1,
                };
            }

            var deleteRemaining = newDelete - positionDelta - oldDelete;
            var deleteRemainingOrZero = deleteRemaining < 0 ? 0 : deleteRemaining;
            
            return new DocumentAction
            {
                Client = newAction.Client,
                Delete = positionDelta + oldInsertLength + deleteRemainingOrZero,
                Insert = newAction.Insert + (oldAction.Insert ?? ""),
                Position = newAction.Position,
                Revision = oldAction.Revision + 1,
            };
        }
        
        var positionDif = newAction.Position - oldAction.Position;
        if (positionDif > oldDelete) return new DocumentAction
        {
            Client = newAction.Client,
            Delete = newDelete,
            Insert = newAction.Insert,
            Position = newAction.Position + oldNetLength,
            Revision = oldAction.Revision + 1,
        };
        
        var deleteSurplus = newDelete - oldDelete + positionDif;
        var deleteSurplusOrZero = deleteSurplus < 0 ? 0 : deleteSurplus;
        return new DocumentAction
        {
            Client = newAction.Client,
            Delete = deleteSurplusOrZero,
            Insert = newAction.Insert,
            Position = oldAction.Position + oldInsertLength,
            Revision = oldAction.Revision + 1,
        };
    }
}