using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class ICC_customer
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int total_shipping_order { get; set; }

        public List<Address_Billing> AddressBilling { get; set; }
        public List<Adress_Agreement> AddressAgreement { get; set; }
        public List<Address_Install> AddressInstall { get; set; }
        //public string DeliveryAddress { get; set; }
        public int CustomerCaptureCategoryId { get; set; }
        public int ClassId { get; set; }
        public int SegmentationId { get; set; }
        public int PropertyTypeId { get; set; }
        public int TypeId { get; set; }
        public int TitleId { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String Surname { get; set; }
        public String CarOfName { get; set; }
        public String Street { get; set; }
        public String Email { get; set; }
        public String PostalCode { get; set; }
        public String EmergencyPhone { get; set; }
        public String WorkPhone { get; set; }
        public String MobilePhone { get; set; }
        public String HomePhone { get; set; }
        public String EmergencyContactName { get; set; }
        public String EmergencyContactAddress { get; set; }
        public String EmergencyContactRelationship { get; set; }
        public String EmergencyContactPhone { get; set; }
        public String GeoLocation { get; set; }
        public String CreditCard { get; set; }
        public int CountryId { get; set; }
        public String BigCity { get; set; }
        public String SmallCity { get; set; }
        public String BirthDate { get; set; }
        public int ReferenceTypeKey { get; set; }
        public String ReferenceNumber { get; set; }
        public int ValidAddressId { get; set; }
        public String HouseNumberAlpha { get; set; }
        public String Flat { get; set; }
        public int HouseNumberNumeric { get; set; }
        public String Address_line2 { get; set; }
        public String Address_line3 { get; set; }
        public String Landmark { get; set; }
        public String Directions { get; set; }

        public int tipe_pembayaran { get; set; }
        public String BankName { get; set; }
        public String FaName { get; set; }
        public int MOPId { get; set; }
        public String CreditCardNumber { get; set; }
        public String ExpirationDate_CC { get; set; }
        public int Payment_date { get; set; }
        public String CreditCardFirstName { get; set; }
        public String CreditCardLastName { get; set; }
        public int ProxyCodeId { get; set; }
        public String ProxyCode { get; set; }
        public String SecurityCode { get; set; }
        public String Remark { get; set; }
        public string IBAN { get; set; }
        public string bank_account_id { get; set; }
        public string bank_code { get; set; }
        public string swift_code { get; set; }
        public int invoicing_profile_id { get; set; }
        public int invoicing_profile_method { get; set; }

        public int the_total_com_prod { get; set; }
        public List<int> the_list_com_prod_id { get; set; }

        public int the_total_promo { get; set; }
        public List<int> the_list_promo { get; set; }

        public int the_total_segmentation { get; set; }
        public List<int> the_segmentation_list { get; set; }

        public int the_total_finance_option_id { get; set; }
        public List<int> the_finance_option_id_list { get; set; }

        public String charge_period { get; set; }

    }
}

