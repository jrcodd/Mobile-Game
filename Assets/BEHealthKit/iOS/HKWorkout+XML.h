//
//  HKWorkout+XML.h
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import <HealthKit/HealthKit.h>

NS_ASSUME_NONNULL_BEGIN

/*! @brief 				Internal category to build an HKWorkout from XML.
*/
@interface HKWorkout (XML)

/*! @brief 				build an HKWorkout from XML.
 	@param xml			xml.
*/
+ (NSArray *)quantitySamplesFromXML:(NSDictionary *)xml;

@end

NS_ASSUME_NONNULL_END
