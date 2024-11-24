//
//  HKWorkoutBuilder+XML.m
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import "HKWorkoutBuilder+XML.h"
#import "XMLDictionary.h"
#import "NSDate+bridge.h"
#import "HKWorkout+XML.h"
#import "BEHealthKit.h"


@implementation HKWorkoutBuilder (XML)

/*
 <workoutBuilder>
   <startDate>1709775967.4017</startDate>
   <endDate>1709776867.4017</endDate>
   <configuration>
	 <workoutConfiguration>
	   <activityType>52</activityType>
	 </workoutConfiguration>
   </configuration>
   <workouts>
	 <workout>
	   <duration>900</duration>
	   <totalDistance unit="mi">0.5</totalDistance>
	   <totalEnergyBurned unit="Cal">75</totalEnergyBurned>
	   <activityType>52</activityType>
	   <startDate>1709775967.4017</startDate>
	   <endDate>1709776867.4017</endDate>
	 </workout>
	 <workout>
	   <duration>900</duration>
	   <totalDistance unit="mi">0.5</totalDistance>
	   <totalEnergyBurned unit="Cal">75</totalEnergyBurned>
	   <activityType>52</activityType>
	   <startDate>1709775967.4017</startDate>
	   <endDate>1709776867.4017</endDate>
	 </workout>
   </workouts>
 </workoutBuilder>
 */
+ (void)buildWorkoutFromXMLString:(NSString *)xmlString healthStore:(HKHealthStore *)store completion:(void (^)(BOOL success, HKWorkoutBuilder *builder, NSError *error))completion;
{
	NSDictionary *xml = [NSDictionary dictionaryWithXMLString:xmlString];
	NSString *root = xml[@"__name"];
	if ([root isEqualToString:@"workoutBuilder"]) {
		NSDate *startDate = [NSDate dateFromBridgeFormat:xml[@"startDate"]];
		NSDate *endDate = [NSDate dateFromBridgeFormat:xml[@"endDate"]];

		NSString *activityID = xml[@"configuration"][@"workoutConfiguration"][@"activityType"];
		HKWorkoutActivityType activityType = (HKWorkoutActivityType)[activityID integerValue];
		HKWorkoutConfiguration *config = [[HKWorkoutConfiguration alloc] init];
		config.activityType = activityType;

		HKWorkoutBuilder *builder = [[HKWorkoutBuilder alloc] initWithHealthStore:store configuration:config device:nil];
		__block HKWorkoutBuilder *_builder = builder;

		// begin building
		[builder beginCollectionWithStartDate:startDate completion:^(BOOL success, NSError * _Nullable beginError) {
			if (!success) {
				completion(false, builder, beginError);
				return;
			}
			
			
			// add samples
			NSMutableArray *workouts = [NSMutableArray array];
			id workoutData = xml[@"workouts"][@"workout"];
			if ([workoutData isKindOfClass:[NSArray class]]) {
				for (NSDictionary *workout in (NSArray *)workoutData) {
					NSArray *samples = [HKWorkout quantitySamplesFromXML:workout];
					[workouts addObjectsFromArray:samples];
				}
				
				
			} else {
				NSArray *samples = [HKWorkout quantitySamplesFromXML:workoutData];
				[workouts addObjectsFromArray:samples];
			}
			
			HKWorkoutBuilder *builder = _builder;
			[builder addSamples:workouts completion:^(BOOL success, NSError * _Nullable addError) {
				if (!success) {
					__block HKWorkoutBuilder *b = _builder;
					completion(false, b, addError);
					return;
				}
				
				HKWorkoutBuilder *builder = _builder;
				[builder endCollectionWithEndDate:endDate completion:^(BOOL success, NSError * _Nullable endError) {
					if (!success) {
						__block HKWorkoutBuilder *b = _builder;
						completion(false, b, endError);
						return;
					}
					
					HKWorkoutBuilder *builder = _builder;
					[builder finishWorkoutWithCompletion:^(HKWorkout * _Nullable_result workout, NSError * _Nullable finishError) {
						__block HKWorkoutBuilder *b = _builder;

						if (!success) {
							completion(false, b, finishError);
							return;
						}
						
						completion(true, b, nil);
					}];
				}];
			}];
		}];
	}
}

@end
