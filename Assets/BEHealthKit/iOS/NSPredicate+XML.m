//
//  NSPredicate+XML.m
//  UnityFramework
//
//  Created by greay on 5/9/23.
//

#import "NSPredicate+XML.h"
#import "XMLDictionary.h"

#import <HealthKit/HealthKit.h>

@implementation NSPredicate (XML)

+ (instancetype)predicateWithBridgeFormat:(NSString *)string withKey:(NSString *)keyString value:(id)value
{
	if ([string isEqualToString:@"metadata.HKMetadataKeyWasUserEntered != YES"]) {
		return [NSPredicate predicateWithFormat:@"metadata.%K != YES", HKMetadataKeyWasUserEntered];
	} else {
		if (value && ![value isEqual:@""]) {
			if ([value isKindOfClass:[NSArray class]]) {
				value = [NSSet setWithArray:value];
			}
			return [NSPredicate predicateWithFormat:string, keyString, value];
		} else {
			return [NSPredicate predicateWithFormat:string, keyString];
		}
	}
}

+ (instancetype)predicateFromXMLString:(NSString *)xmlString
{
	NSDictionary *xml = [NSDictionary dictionaryWithXMLString:xmlString];
	NSString *root = xml[@"__name"];
	if ([root isEqualToString:@"predicate"]) {
		return [NSPredicate predicateWithBridgeFormat:xml[@"format"] withKey:xml[@"key"] value:xml[@"value"]];
	} else if ([root isEqualToString:@"compoundPredicate"]) {
		NSMutableArray *subPredicates = [NSMutableArray array];
		for (NSDictionary *predicate in xml[@"subPredicates"]) {
			[subPredicates addObject:[NSPredicate predicateWithBridgeFormat:predicate[@"format"] withKey:predicate[@"key"] value:predicate[@"value"]]];
		}
		if ([xml[@"type"] isEqualToString:@"AndPredicate"]) {
			return [NSCompoundPredicate orPredicateWithSubpredicates:subPredicates];
		} else if ([xml[@"type"] isEqualToString:@"OrPredicate"]) {
			return [NSCompoundPredicate orPredicateWithSubpredicates:subPredicates];
		} else if ([xml[@"type"] isEqualToString:@"NotPredicate"]) {
			return [NSCompoundPredicate orPredicateWithSubpredicates:subPredicates];
		}
	}

	return nil;
}

@end
