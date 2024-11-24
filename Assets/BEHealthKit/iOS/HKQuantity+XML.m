//
//  HKQuantity+XML.m
//  UnityFramework
//
//  Created by greay on 3/4/24.
//

#import "HKQuantity+XML.h"
#import "XMLDictionary.h"
#import "NSDate+bridge.h"

@implementation HKQuantity (XML)

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
+ (instancetype)quantityFromXML:(NSDictionary *)xml
{
	NSString *unit = xml[@"_unit"];
	NSNumber *value = xml[@"__text"];
	HKQuantity *quantity = [HKQuantity quantityWithUnit:[HKUnit unitFromString:unit] doubleValue:[value doubleValue]];
	return quantity;
}

@end
