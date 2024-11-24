//
//  HKWorkout+XML.m
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import "HKWorkout+XML.h"
#import "XMLDictionary.h"
#import "NSDate+bridge.h"
#import "HKQuantity+XML.h"

@implementation HKWorkout (XML)

/*
 <workout>
   <duration>900</duration>
   <totalDistance unit="mi">0.5</totalDistance>
   <totalEnergyBurned unit="Cal">75</totalEnergyBurned>
   <activityType>52</activityType>
   <startDate>1709775967.4017</startDate>
   <endDate>1709776867.4017</endDate>
 </workout>
 */
+ (NSArray *)quantitySamplesFromXML:(NSDictionary *)xml
{
	NSDate *startDate = [NSDate dateFromBridgeFormat:xml[@"startDate"]];
	NSDate *endDate = [NSDate dateFromBridgeFormat:xml[@"endDate"]];

	NSString *activityID = xml[@"activityType"];
	HKWorkoutActivityType activityType = (HKWorkoutActivityType)[activityID integerValue];
	
	NSMutableArray *samples = [NSMutableArray array];
	HKQuantity *cal = [HKQuantity quantityFromXML:xml[@"totalEnergyBurned"]];
	if ([cal doubleValueForUnit:[HKUnit largeCalorieUnit]] > 0) {
		HKQuantitySample *sample = [HKQuantitySample quantitySampleWithType:[HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierActiveEnergyBurned] quantity:cal startDate:startDate endDate:endDate];
		[samples addObject:sample];
	}
	HKQuantity *d = [HKQuantity quantityFromXML:xml[@"totalDistance"]];
	if ([d doubleValueForUnit:[HKUnit meterUnit]] > 0) {
		HKQuantityType *type = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning];
		if (activityType == HKWorkoutActivityTypeCycling || activityType == HKWorkoutActivityTypeHandCycling) {
			type = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceCycling];
		} else if (activityType == HKWorkoutActivityTypeWheelchairRunPace || activityType == HKWorkoutActivityTypeWheelchairWalkPace) {
			type = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWheelchair];
		} else if (activityType == HKWorkoutActivityTypeSwimming || activityType == HKWorkoutActivityTypeWaterFitness || activityType == HKWorkoutActivityTypeWaterPolo) {
			type = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceSwimming];
		} else if (activityType == HKWorkoutActivityTypeDownhillSkiing || activityType == HKWorkoutActivityTypeSnowboarding || activityType == HKWorkoutActivityTypeSnowSports) {
			type = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceDownhillSnowSports];
		}
		HKQuantitySample *sample = [HKQuantitySample quantitySampleWithType:type quantity:d startDate:startDate endDate:endDate];
		[samples addObject:sample];
	}

//	HKWorkout *sample = [HKWorkout workoutWithActivityType:activityType startDate:startDate endDate:endDate duration:0 totalEnergyBurned:cal totalDistance:d metadata:nil];
	
	return samples;
}

@end
