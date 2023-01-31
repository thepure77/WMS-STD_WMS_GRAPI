using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class OwnerViewModel
    {
        public Guid OwnerIndex { get; set; }



        public string OwnerId { get; set; }



        public string OwnerName { get; set; }


        public string OwnerAddress { get; set; }


        public string OwnerTypeName { get; set; }


        public string OwnerTaxID { get; set; }


        public string OwnerEmail { get; set; }


        public string OwnerFax { get; set; }


        public string OwnerTel { get; set; }


        public string OwnerMobile { get; set; }


        public string OwnerBarcode { get; set; }


        public string ContactPerson { get; set; }


        public string ContactPerson2 { get; set; }


        public string ContactPerson3 { get; set; }


        public string ContactTel { get; set; }


        public string ContactTel2 { get; set; }


        public string ContactTel3 { get; set; }


        public string ContactEmail { get; set; }


        public string ContactEmail2 { get; set; }


        public string ContactEmail3 { get; set; }


        public string SubDistrictName { get; set; }


        public string DistrictName { get; set; }


        public string ProvinceName { get; set; }


        public string PostCodeName { get; set; }


        public string CountryName { get; set; }

        public Guid OwnerTypeIndex { get; set; }

        public Guid? SubDistrictIndex { get; set; }

        public Guid? DistrictIndex { get; set; }

        public Guid? ProvinceIndex { get; set; }

        public Guid? CountryIndex { get; set; }

        public Guid? PostCodeIndex { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? StatusId { get; set; }


        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CancelBy { get; set; }


        public DateTime? CancelDate { get; set; }

        public string key { get; set; }
        public string name { get; set; }

    }

    public class OwnerViewModelV2
    {


        public Guid owner_Index { get; set; }

        public string owner_Id { get; set; }


        public string owner_Name { get; set; }


        public string owner_Address { get; set; }

        public Guid ownerType_Index { get; set; }

        public Guid? subDistrict_Index { get; set; }

        public Guid? district_Index { get; set; }

        public Guid? province_Index { get; set; }

        public Guid? country_Index { get; set; }

        public Guid? postcode_Index { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }
        public string owner_TaxID { get; set; }


        public string key { get; set; }
        public string name { get; set; }


    }

    public class OwnerViewModelV3
    {
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string owner_SecondName { get; set; }
        public string owner_Address { get; set; }

        public Guid? ownerType_Index { get; set; }
        public string ownerType_Id { get; set; }
        public string ownerType_Name { get; set; }

        public Guid? province_Index { get; set; }
        public string province_Id { get; set; }
        public string province_Name { get; set; }
        public Guid? country_Index { get; set; }
        public string country_Id { get; set; }
        public string country_Name { get; set; }
        public Guid? subDistrict_Index { get; set; }
        public string subDistrict_Id { get; set; }
        public string subDistrict_Name { get; set; }
        public Guid? district_Index { get; set; }
        public string district_Id { get; set; }
        public string district_Name { get; set; }
        public Guid? postcode_Index { get; set; }
        public string postcode_Id { get; set; }
        public string postcode_Name { get; set; }

        public string owner_TaxID { get; set; }
        public string owner_Email { get; set; }
        public string owner_Fax { get; set; }
        public string owner_Tel { get; set; }
        public string owner_Mobile { get; set; }
        public string owner_Barcode { get; set; }
        public string contact_Person { get; set; }
        public string contact_Person2 { get; set; }
        public string contact_Person3 { get; set; }
        public string contact_Tel { get; set; }
        public string contact_Tel2 { get; set; }
        public string contact_Tel3 { get; set; }
        public string contact_Email { get; set; }
        public string contact_Email2 { get; set; }
        public string contact_Email3 { get; set; }

        public string ref_No1 { get; set; }
        public string ref_No2 { get; set; }
        public string ref_No3 { get; set; }
        public string ref_No4 { get; set; }
        public string ref_No5 { get; set; }
        public string remark { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string key { get; set; }

        public string value1 { get; set; }

    }
}
