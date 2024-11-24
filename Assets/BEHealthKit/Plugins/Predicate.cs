using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BeliefEngine.HealthKit
{

/*! @brief 		A way to create simple NSPredicates.
	@details	NSPredicate (https://developer.apple.com/documentation/foundation/nspredicate) is a definition of logical conditions used to constrain a search either for a fetch or for in-memory filtering.
 */
public class Predicate : System.Object
{
	public string formatString; /*!< @brief The predicate's format string */
	public string key; /*!< @brief The predicate's key string (%K in the format string) */
	public System.Object value; /*!< @brief The predicate's value string (%@ in the format string) */

	public Predicate() {}

	/*! @brief		  Construct a Predicate with a format string.
		@param format the format string to use.
	 */
	public Predicate(string format) {
		//[NSPredicate predicateWithFormat:@"metadata.%K != YES", HKMetadataKeyWasUserEntered];
		this.formatString = format;
	}

	/*! @brief		  Construct a Predicate with a format string.
		@param format the format string to use.
		@param key    the predicate's key string (%K in the format string) 
	 */
	public Predicate(string format, string key) {
		//[NSPredicate predicateWithFormat:@"metadata.%K != YES", HKMetadataKeyWasUserEntered];
		this.formatString = format;
		this.key = key;
	}

	/*! @brief		  Construct a Predicate with a format string.
		@param format the format string to use.
		@param key    the predicate's key string (%K in the format string) 
		@param value  the predicate's value string (%@ in the format string)
	 */
	public Predicate(string format, string key, System.Object value) {
		//[NSPredicate predicateWithFormat:@"metadata.%K != YES", HKMetadataKeyWasUserEntered];
		this.formatString = format;
		this.key = key;
		this.value = value;
	}

	/*! @brief generates XML Element to convert the Predicate to an NSPredicate on the Obj-C side.
	 */
	public virtual XElement ToXML() {
		XElement el = new XElement("predicate",
			new XElement("format", this.formatString)
		);
		if (this.key != null) {
			el.Add(new XElement("key", this.key));
		}
		if (this.value != null) {
			if (this.value is List<string> listValues) {
				foreach (string str in listValues) {
					el.Add(new XElement("value", str));
				}
			} else if (this.value is string[] arrayValues) {
				foreach (string str in arrayValues) {
					el.Add(new XElement("value", str));
				}
			} else {
				el.Add(new XElement("value", value));
			}
		}
		return el;
	}

	/*! @brief generates an XML string, to convert the Predicate to an NSPredicate on the Obj-C side.
	 */
	public string ToXMLString()
	{
		return this.ToXML().ToString();
	}
}

public enum CompoundPredicateType
{
	NotPredicate,
	AndPredicate,
	OrPredicate
}

/*! @brief 		A way to create NSCompoundPredicates.
	@details	NSCompoundPredicate (https://developer.apple.com/documentation/foundation/nscompoundpredicate) combines multiple subPredicates using AND, NOT, or OR.
 */
public class CompoundPredicate : Predicate
{
	public CompoundPredicateType predicateType; /*!< @brief The logical type of compound Predicate */
	public List<Predicate> subPredicates; /*!< @brief The compound Predicate's subpredicates */

	/*! @brief		          Construct a Predicate with a logical type, and enemyConrollerList of subpredicates.
		@param predicateType  the logical operator used to combine the subpredicates
		@param subPredicates  a enemyConrollerList of predicates to combine
	 */
	public CompoundPredicate(CompoundPredicateType predicateType, List<Predicate> subPredicates) {
		this.predicateType = predicateType;
		this.subPredicates = subPredicates;
	}

	/*! @brief generates XML Element to convert the Predicate to an NSPredicate on the Obj-C side.
	 */
	override public XElement ToXML()
	{
		XElement el = new XElement("compoundPredicate",
			new XElement("type", this.predicateType.ToString()),
			new XElement("subPredicates", this.subPredicates.Select(i => i.ToXMLString()))
		);
		return el;
	}
}

}