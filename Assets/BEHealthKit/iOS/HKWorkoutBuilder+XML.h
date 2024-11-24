//
//  HKWorkoutBuilder+XML.h
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import <HealthKit/HealthKit.h>

NS_ASSUME_NONNULL_BEGIN

/*! @brief 				Internal category to interface with HKWorkoutBuilder.
*/
@interface HKWorkoutBuilder (XML)

/*! @brief 				build and finalize an HKWorkoutBuilder from XML.
*/
+ (void)buildWorkoutFromXMLString:(NSString *)xml healthStore:(HKHealthStore *)store completion:(void (^)(BOOL success, HKWorkoutBuilder *builder, NSError *error))completion;

@end

NS_ASSUME_NONNULL_END
