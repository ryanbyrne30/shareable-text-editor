export class DateUtil {
	public static dateOffset = (
		date: Date,
		offset: { seconds?: number; minutes?: number; days?: number }
	) => {
		const seconds = offset.seconds ?? 0;
		const minutes = offset.minutes ?? 0;
		const days = offset.days ?? 0;
		const dup = new Date(date);
		dup.setSeconds(seconds);
		dup.setMinutes(minutes);
		dup.setDate(days);
		return dup;
	};
}
