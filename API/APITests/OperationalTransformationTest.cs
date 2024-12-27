using API.Document;
using API.Document.Domain;

namespace APITests;

public class Tests
{
    private static DocumentAction CreateAction(int pos, string? insert, int? delete)
    {
        return new DocumentAction(0, pos, insert, delete);
    }

    private static void AssertExpectedString(string baseString, string expectedString, DocumentAction oldAction, DocumentAction transformedAction)
    {
        var oldString = oldAction.Apply(baseString);
        var newString = transformedAction.Apply(oldString);
        Assert.That(newString, Is.EqualTo(expectedString));
        
    }

    [Test]
    public void InsertOverInsert_AtPositionBefore_ShouldInsertBefore()
    {
        var oldAction = CreateAction(3, "aaa", null);
        var newAction = CreateAction(2, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "01bbb2aaa3456";
        var receivedAction = OperationalTransformation.InsertOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverInsert_AtPositionEqual_ShouldInsertAfter()
    {
        var oldAction = CreateAction(1, "aaa", null);
        var newAction = CreateAction(1, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0aaabbb123456";
        var receivedAction = OperationalTransformation.InsertOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverInsert_AtPositionAfter_ShouldInsertAfter()
    {
        var oldAction = CreateAction(1, "aaa", null);
        var newAction = CreateAction(3, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0aaa12bbb3456";
        var receivedAction = OperationalTransformation.InsertOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverInsert_AtPositionWayBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(6, "aaa", null);
        var newAction = CreateAction(2, null, 2);
        const string baseString = "0123456";
        const string expectedString = "0145aaa6";
        var receivedAction = OperationalTransformation.DeleteOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverInsert_AtPositionJustBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(3, "aaa", null);
        var newAction = CreateAction(2, null, 2);
        const string baseString = "0123456";
        const string expectedString = "01aaa456";
        var receivedAction = OperationalTransformation.DeleteOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverInsert_AtPositionEqual_ShouldDeleteAfterInsert()
    {
        var oldAction = CreateAction(3, "aaa", null);
        var newAction = CreateAction(3, null, 2);
        const string baseString = "0123456";
        const string expectedString = "012aaa56";
        var receivedAction = OperationalTransformation.DeleteOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverInsert_AtPositionAfter_ShouldDeleteAfterInsert()
    {
        var oldAction = CreateAction(1, "aaa", null);
        var newAction = CreateAction(3, null, 2);
        const string baseString = "0123456";
        const string expectedString = "0aaa1256";
        var receivedAction = OperationalTransformation.DeleteOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverInsert_AtPositionWayBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(6, "aaa", null);
        var newAction = CreateAction(1, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0bbb345aaa6";
        var receivedAction = OperationalTransformation.UpdateOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverInsert_AtPositionAfter_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, "aaa", null);
        var newAction = CreateAction(2, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0aaa1bbb456";
        var receivedAction = OperationalTransformation.UpdateOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverInsert_AtPositionEqual_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, "aaa", null);
        var newAction = CreateAction(1, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0aaabbb3456";
        var receivedAction = OperationalTransformation.UpdateOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverInsert_AtPositionJustBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(3, "aaa", null);
        var newAction = CreateAction(2, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "01bbbaaa456";
        var receivedAction = OperationalTransformation.UpdateOverInsert(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverDelete_AtPositionBefore_ShouldInsertBefore()
    {
        var oldAction = CreateAction(5, null, 2);
        var newAction = CreateAction(1, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0bbb1234";
        var receivedAction = OperationalTransformation.InsertOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverDelete_AtPositionEqual_ShouldInsertAt()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(1, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0bbb3456";
        var receivedAction = OperationalTransformation.InsertOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverDelete_AtPositionJustAfter_ShouldInsertAfter()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(2, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0bbb3456";
        var receivedAction = OperationalTransformation.InsertOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverDelete_AtPositionWayAfter_ShouldInsertAfter()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(5, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "034bbb56";
        var receivedAction = OperationalTransformation.InsertOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionWayBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(5, null, 1);
        var newAction = CreateAction(0, null, 4);
        const string baseString = "0123456";
        const string expectedString = "46";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionJustBeforeExceeding_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(2, null, 1);
        var newAction = CreateAction(1, null, 4);
        const string baseString = "0123456";
        const string expectedString = "056";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionJustBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(2, null, 4);
        var newAction = CreateAction(1, null, 2);
        const string baseString = "0123456";
        const string expectedString = "06";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionAt_ShouldDeleteAt()
    {
        var oldAction = CreateAction(2, null, 4);
        var newAction = CreateAction(2, null, 2);
        const string baseString = "0123456";
        const string expectedString = "016";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionWayAfter_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(5, null, 1);
        const string baseString = "0123456";
        const string expectedString = "0346";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverDelete_AtPositionJustAfter_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(2, null, 3);
        const string baseString = "0123456";
        const string expectedString = "056";
        var receivedAction = OperationalTransformation.DeleteOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionWayBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(5, null, 2);
        var newAction = CreateAction(1, "bbb", 1);
        const string baseString = "0123456";
        const string expectedString = "0bbb234";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionWayAfter_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, null, 2);
        var newAction = CreateAction(5, "bbb", 1);
        const string baseString = "0123456";
        const string expectedString = "034bbb6";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionJustBeforeExceeding_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(3, null, 1);
        var newAction = CreateAction(2, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "01bbb56";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionJustBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(3, null, 3);
        var newAction = CreateAction(2, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "01bbb6";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionJustAfterExceeding_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(2, null, 3);
        var newAction = CreateAction(3, "bbb", 1);
        const string baseString = "0123456";
        const string expectedString = "01bbb56";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionJustAfter_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(2, null, 1);
        var newAction = CreateAction(3, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "01bbb6";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverDelete_AtPositionEqual_ShouldUpdateAt()
    {
        var oldAction = CreateAction(2, null, 1);
        var newAction = CreateAction(2, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "01bbb56";
        var receivedAction = OperationalTransformation.UpdateOverDelete(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverUpdate_AtPositionBefore_ShouldInsertBefore()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(1, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "0bbb1aaa3456";
        var receivedAction = OperationalTransformation.InsertOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverUpdate_AtPositionEqual_ShouldInsertAfter()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(2, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "01aaabbb3456";
        var receivedAction = OperationalTransformation.InsertOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverUpdate_AtPositionJustAfter_ShouldInsertAfter()
    {
        var oldAction = CreateAction(2, "aaa", 3);
        var newAction = CreateAction(3, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "01aaabbb56";
        var receivedAction = OperationalTransformation.InsertOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void InsertOverUpdate_AtPositionWayAfter_ShouldInsertAfter()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(5, "bbb", null);
        const string baseString = "0123456";
        const string expectedString = "01aaa34bbb56";
        var receivedAction = OperationalTransformation.InsertOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionWayBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(5, "aaa", 1);
        var newAction = CreateAction(1, null, 2);
        const string baseString = "0123456";
        const string expectedString = "034aaa6";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionJustBefore_ShouldDeleteBefore()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(1, null, 3);
        const string baseString = "0123456";
        const string expectedString = "0aaa456";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionAt_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(2, null, 3);
        const string baseString = "0123456";
        const string expectedString = "01aaa56";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionWayAfter_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(1, "aaa", 1);
        var newAction = CreateAction(4, null, 2);
        const string baseString = "0123456";
        const string expectedString = "0aaa236";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionJustAfter_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(1, "aaa", 3);
        var newAction = CreateAction(2, null, 1);
        const string baseString = "0123456";
        const string expectedString = "0aaa456";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void DeleteOverUpdate_AtPositionJustAfterExceeding_ShouldDeleteAfter()
    {
        var oldAction = CreateAction(1, "aaa", 1);
        var newAction = CreateAction(2, null, 3);
        const string baseString = "0123456";
        const string expectedString = "0aaa56";
        var receivedAction = OperationalTransformation.DeleteOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionWayBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(5, "aaa", 1);
        var newAction = CreateAction(1, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0bbb34aaa6";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionWayAfter_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, "aaa", 1);
        var newAction = CreateAction(5, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0aaa234bbb";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionJustBefore_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(2, "aaa", 3);
        var newAction = CreateAction(1, "bbb", 2);
        const string baseString = "0123456";
        const string expectedString = "0bbbaaa56";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionJustBeforeExceeding_ShouldUpdateBefore()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(1, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "0bbbaaa456";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionJustAfter_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, "aaa", 3);
        var newAction = CreateAction(2, "bbb", 1);
        const string baseString = "0123456";
        const string expectedString = "0aaabbb456";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionJustAfterExceeding_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(1, "aaa", 1);
        var newAction = CreateAction(2, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "0aaabbb56";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionEqualExceeding_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(2, "aaa", 1);
        var newAction = CreateAction(2, "bbb", 3);
        const string baseString = "0123456";
        const string expectedString = "01aaabbb56";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void UpdateOverUpdate_AtPositionEqual_ShouldUpdateAfter()
    {
        var oldAction = CreateAction(2, "aaa", 3);
        var newAction = CreateAction(2, "bbb", 1);
        const string baseString = "0123456";
        const string expectedString = "01aaabbb56";
        var receivedAction = OperationalTransformation.UpdateOverUpdate(newAction, oldAction);
        AssertExpectedString(baseString, expectedString, oldAction, receivedAction);
    }
    
    [Test]
    public void Transform_ShouldSyncCorrectly()
    {
        const string baseString = "0123456";
        List<DocumentAction> completedActions =
        [
            CreateAction(3, null, 2), // 01256
            CreateAction(1, "aaa", 1), // 0aaa256
            CreateAction(3, "z", null), // 0aaza256
        ];
        var newAction = new DocumentAction(0, 2, "bbb", 1);
        const string expected = "0aazabbb56";
        var currentDoc = completedActions.Aggregate(baseString, (s, action) => action.Apply(s));
        
        var transformedAction = OperationalTransformation.Transform(newAction, completedActions);
        var received = transformedAction.Apply(currentDoc);
        Assert.That(received, Is.EqualTo(expected));
    }
    
    [Test]
    public void Transform_ShouldSyncCorrectly2()
    {
        const string baseString = "0123456";
        List<DocumentAction> completedActions =
        [
            CreateAction(1, "aaa", 2), // 0aaa3456 
            CreateAction(3, "z", null), // 0aaza3456
        ];
        var newAction = new DocumentAction(0, 1, "bbb", 1);
        const string expected = "0aazabbb3456";
        var currentDoc = completedActions.Aggregate(baseString, (s, action) => action.Apply(s));
        
        var transformedAction = OperationalTransformation.Transform(newAction, completedActions);
        var received = transformedAction.Apply(currentDoc);
        Assert.That(received, Is.EqualTo(expected));
    }
}