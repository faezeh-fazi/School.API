using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services.General
{

    public class PropertyMappingService : IPropertyMappingService
    {
       
        private Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"Name",new PropertyMappingValue(new List<string>(){ "Name"}) },
                {"Mobile",new PropertyMappingValue(new List<string>(){ "Mobile"}) },
                {"Email",new PropertyMappingValue(new List<string>(){ "Email"}) },
                {"Nationality",new PropertyMappingValue(new List<string>(){ "Nationality"}) }
            };

        private Dictionary<string, PropertyMappingValue> _userListPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"Name",new PropertyMappingValue(new List<string>(){ "Name"}) }
            };

        private Dictionary<string, PropertyMappingValue> _carSellPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"Date",new PropertyMappingValue(new List<string>(){ "Date"}) },
                };

       private Dictionary<string, PropertyMappingValue> _customerCarSellPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                };

        private Dictionary<string, PropertyMappingValue> _FeeTaxPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                };

        private Dictionary<string, PropertyMappingValue> _ExpensePropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                };


        private Dictionary<string, PropertyMappingValue> _expensesPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"TypeId",new PropertyMappingValue(new List<string>(){ "TypeName"}) },
                {"Date",new PropertyMappingValue(new List<string>(){ "Date"}) },
           };

        private Dictionary<string, PropertyMappingValue> _generalexpensesPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"TypeId",new PropertyMappingValue(new List<string>(){ "TypeName"}) },
           };

        private Dictionary<string, PropertyMappingValue> _paymentPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) }
           };

        private Dictionary<string, PropertyMappingValue> _carPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
               {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
               {"Year",new PropertyMappingValue(new List<string>(){ "Year"}) },
               {"Condition",new PropertyMappingValue(new List<string>(){ "Condition"}) },
               {"Doors",new PropertyMappingValue(new List<string>(){ "Seats"}) },
               {"Seats",new PropertyMappingValue(new List<string>(){ "Doors"}) }
            };

        

        private Dictionary<string, PropertyMappingValue> _inventoryProductPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"ProductName",new PropertyMappingValue(new List<string>(){ "ProductName"}) },
                           {"ProductCode",new PropertyMappingValue(new List<string>(){ "ProductCode"}) }
                };

        private Dictionary<string, PropertyMappingValue> _inventoryPurchasePropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"Supplier",new PropertyMappingValue(new List<string>(){ "Supplier"}) }
                };

        private Dictionary<string, PropertyMappingValue> _inventoryPurchaseListPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"Supplier",new PropertyMappingValue(new List<string>(){ "Supplier"}) }
                };

        private Dictionary<string, PropertyMappingValue> _inventorySellPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"Customer",new PropertyMappingValue(new List<string>(){ "Customer"}) }
                };

                 private Dictionary<string, PropertyMappingValue> _inventorySellListPropertyMapping =
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                           {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
                           {"Customer",new PropertyMappingValue(new List<string>(){ "Customer"}) }
                };

        private Dictionary<string, PropertyMappingValue> _exchangePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
               {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) }
            };

        private Dictionary<string, PropertyMappingValue> _galleryInsurancePropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
           };


        private Dictionary<string, PropertyMappingValue> _customerInsurancePropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               {"Id",new PropertyMappingValue(new List<string>(){ "Id"}) },
           };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            //propertyMappings.Add(new PropertyMapping<UserViewDto, Staff>(_userPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<UserListViewDto, Staff>(_userListPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<CarViewDto, Car>(_carPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<FeeTaxListDto, FeeTax>(_FeeTaxPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<ExpenseListDto, Expenses>(_ExpensePropertyMapping));
            //propertyMappings.Add(new PropertyMapping<ExpensesViewDto, Expenses>(_expensesPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<GeneralExpenseViewDto, GeneralExpenses>(_generalexpensesPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<PaymentViewDto, Payment>(_expensesPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<ExchangeViewDto, ExchangeRecord>(_exchangePropertyMapping));
            //propertyMappings.Add(new PropertyMapping<SellListViewDto, CarSale>(_carSellPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<CustomerCarSellListDto, CarSale>(_customerCarSellPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<ProductViewDto, Product>(_inventoryProductPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<InventoryPurchaseListDto, InventoryPurchase>(_inventoryPurchasePropertyMapping));
            //propertyMappings.Add(new PropertyMapping<InventoryPurchaseViewDto, InventoryPurchase>(_inventoryPurchasePropertyMapping));
            //propertyMappings.Add(new PropertyMapping<InventorySellViewDto, InventorySell>(_inventorySellPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<InventorySellListDto, InventorySell>(_inventorySellListPropertyMapping));
            //propertyMappings.Add(new PropertyMapping<GalleryInsuranceListViewDto, GalleryInsurance>(_galleryInsurancePropertyMapping));
            //propertyMappings.Add(new PropertyMapping<CustomerInsuranceListViewDto, CustomerInsurance>(_customerInsurancePropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;

        }
    }
}
