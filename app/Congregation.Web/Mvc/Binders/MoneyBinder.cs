using System;
using System.Web.Mvc;

namespace Congregation.Web.Mvc.Binders
{
	// Sample Model Binder
	//public class MoneyBinder : IModelBinder
	//{
	//    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
	//        ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".Amount");

	//        string attemptedValue = value.AttemptedValue;

	//        decimal amount;
	//        bool couldAmountBeParsed = decimal.TryParse(attemptedValue, out amount);

	//        return couldAmountBeParsed
	//            ? new Money(amount)
	//            : null;
	//    }
	//}

	//public class Money
	//{
	//    protected Money() { }

	//    public Money(decimal amount) {
	//        Amount = amount;
	//    }

	//    public virtual decimal Amount { get; set; }

	//    public override string ToString() {
	//        return String.Format("{0:c}", Amount);
	//    }
	//}

}