//
//  HKQuantity+XML.h
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import <HealthKit/HealthKit.h>

NS_ASSUME_NONNULL_BEGIN

/*! @brief 				Internal category to build an HKQuantity from XML.
*/
@interface HKQuantity (XML)

/*! @brief 				build an HKQuantity from XML.
 	@param xml			xml.
*/
+ (instancetype)quantityFromXML:(NSDictionary *)xml;

@end

NS_ASSUME_NONNULL_END
