export class EnumUtil {
	public static getEnumValue = (enumType: any, enumKey: string): string | undefined => {
		return enumType[enumKey as keyof typeof enumType];
	};
}
