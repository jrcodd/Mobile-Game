//
//  NSPredicate+XML.h
//  UnityFramework
//
//  Created by greay on 5/9/23.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN


/*! @brief 				Internal category to build an NSPredicate from XML.
*/
@interface NSPredicate (XML)

/*! @brief 				build a predicate from XML.
*/
+ (instancetype)predicateFromXMLString:(NSString *)xml;

@end

NS_ASSUME_NONNULL_END
