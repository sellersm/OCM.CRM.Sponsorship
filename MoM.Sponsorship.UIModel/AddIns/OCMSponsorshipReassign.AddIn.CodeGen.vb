﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by Blackbaud.AppFx.SDKVSWizards.dll
'     Version:  4.0.170.0
'     Date:  4/4/2018 4:51:03 PM
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Partial Public NotInheritable Class OCMSponsorshipReassignAddIn
    Inherits Blackbaud.AppFx.UIModeling.Core.RootModelAddIn(Of Global.Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel)

    Friend WithEvents MODEL As Global.Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel
    Friend WithEvents [FORMTITLE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [REVENUECONSTITUENTID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [GIFTRECIPIENT] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [SPONSORSHIPCONSTITUENTID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [SPONSORSHIPREASONID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [SPONSORSHIPPROGRAMID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [PLANNEDENDDATE] As Global.Blackbaud.AppFx.UIModeling.Core.DateField
    Friend WithEvents [SPONSORSHIPLOCATIONID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [SPROPPAGERANGEID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [GENDERCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.GENDERCODES?)
    Friend WithEvents [ISHIVPOSITIVECODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.ISHIVPOSITIVECODES?)
    Friend WithEvents [HASCONDITIONCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.HASCONDITIONCODES?)
    Friend WithEvents [ISORPHANEDCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.ISORPHANEDCODES?)
    Friend WithEvents [SPROPPPROJECTCATEGORYCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [SPONSORSHIPOPPORTUNITYIDCHILD] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [SPONSORSHIPOPPORTUNITYIDPROJECT] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [ISSOLESPONSORSHIP] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [STARTDATE] As Global.Blackbaud.AppFx.UIModeling.Core.DateField
    Friend WithEvents [AMOUNT] As Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
    Friend WithEvents [AUTOPAY] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [PAYMENTMETHODCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.PAYMENTMETHODCODES)
    Friend WithEvents [CONSTITUENTACCOUNTID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [CONSTITUENTACCOUNTIDADDACCOUNTACTION] As Global.Blackbaud.AppFx.UIModeling.Core.ShowAddFormUIAction
    Friend WithEvents [REFERENCENUMBER] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [REFERENCEDATE] As Global.Blackbaud.AppFx.UIModeling.Core.FuzzyDateField
    Friend WithEvents [FREQUENCYCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.FREQUENCYCODES)
    Friend WithEvents [REVENUESCHEDULESTARTDATE] As Global.Blackbaud.AppFx.UIModeling.Core.DateField
    Friend WithEvents [REVENUESCHEDULEENDDATE] As Global.Blackbaud.AppFx.UIModeling.Core.DateField
    Friend WithEvents [CREDITCARDNUMBER] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [CARDHOLDERNAME] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [EXPIRESON] As Global.Blackbaud.AppFx.UIModeling.Core.FuzzyDateField
    Friend WithEvents [CREDITCARDTOKEN] As Global.Blackbaud.AppFx.UIModeling.Core.CreditCardTokenField
    Friend WithEvents [CREDITTYPECODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [RESERVEDOPPORTUNITYIDCHILD] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [RESERVATIONKEY] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [MATCHEDOPPORTUNITYID] As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
    Friend WithEvents [SENDREMINDER] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [FINDERNUMBER] As Global.Blackbaud.AppFx.UIModeling.Core.LongField
    Friend WithEvents [SOURCECODE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [APPEALID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [MAILINGID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [CHANNELCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [REFERENCE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [CATEGORYCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [EXPIRATIONREASONID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [TRANSACTIONCURRENCYID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [BASEEXCHANGERATEID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [EXCHANGERATE] As Global.Blackbaud.AppFx.UIModeling.Core.DecimalField
    Friend WithEvents [BASECURRENCYID] As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
    Friend WithEvents [BASEAMOUNT] As Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
    Friend WithEvents [BASECURRENCYDECIMALDIGITS] As Global.Blackbaud.AppFx.UIModeling.Core.TinyIntField
    Friend WithEvents [BASECURRENCYROUNDINGTYPECODE] As Global.Blackbaud.AppFx.UIModeling.Core.TinyIntField
    Friend WithEvents [BATCHNUMBER] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [REVENUEDEVELOPMENTFUNCTIONCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [DONOTACKNOWLEDGE] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [SEPAMANDATEID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [SEPAMANDATEIDADDASEPAMANDATEACTION] As Global.Blackbaud.AppFx.UIModeling.Core.ShowAddFormUIAction
    Friend WithEvents [CARRYFORWARDPASTDUE] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [CARRYFORWARDPASTDUEAMOUNT] As Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
    Friend WithEvents [OTHERPAYMENTMETHODCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [SELECTOPPORTUNITYID] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS?)
    Friend WithEvents [ISFIXEDTERMSPONSORSHIP] As Global.Blackbaud.AppFx.UIModeling.Core.BooleanField
    Friend WithEvents [RGSCHEDULE] As Global.Blackbaud.AppFx.UIModeling.Core.XmlField
    Friend WithEvents [MOPPORTUNITYNAME] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [MOPPORTUNITYLOCATION] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [MOPPORTUNITYLOOKUPID] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [MOPPORTUNITYGENDER] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [MOPPORTUNITYBIRTHDATE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [MOPPORTUNITYIMAGE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [FINDERNUMBERSTRING] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [INVALIDFINDERNUMBERIMAGE] As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Friend WithEvents [INSTALLMENTS] As Global.Blackbaud.AppFx.UIModeling.Core.CollectionField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormINSTALLMENTSUIModel)
    Friend WithEvents [PAYMENTMETHODCODEUI] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField(Of Blackbaud.AppFx.Sponsorship.UIModel.SponsorshipAddFormUIModel.PAYMENTMETHODCODEUIS)
    Friend WithEvents [GENERALINFORMATION] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [SPONSORSHIPOPPORTUNITY] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [PAYMENTSCHEDULE] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [PAYMENTINFORMATION] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [SPECIALINSTRUCTIONSGROUP] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [MATCHEDOPPORTUNITY] As Global.Blackbaud.AppFx.UIModeling.Core.GroupField
    Friend WithEvents [TAB_SPONSORSHIP] As Global.Blackbaud.AppFx.UIModeling.Core.TabField
    Friend WithEvents [TAB_PAYMENT] As Global.Blackbaud.AppFx.UIModeling.Core.TabField
    Friend WithEvents [TAB_EXPIRATION] As Global.Blackbaud.AppFx.UIModeling.Core.TabField
    Friend WithEvents [FINDOPPORTUNITY] As Global.Blackbaud.AppFx.UIModeling.Core.GenericUIAction
    Friend WithEvents [SOURCECODELOOKUP] As Global.Blackbaud.AppFx.UIModeling.Core.ShowSearchFormUIAction
    Friend WithEvents [CURRENCYACTION] As Global.Blackbaud.AppFx.UIModeling.Core.ShowCustomFormUIAction
    Friend WithEvents [hSPONSORSHIPPROGRAMID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [hSPONSORSHIPLOCATIONID] As Global.Blackbaud.AppFx.UIModeling.Core.SearchListField(Of Guid)
    Friend WithEvents [hGENDERCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField
    Friend WithEvents [hHASCONDITIONCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField
    Friend WithEvents [hISHIVPOSITIVECODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField
    Friend WithEvents [hISORPHANEDCODE] As Global.Blackbaud.AppFx.UIModeling.Core.ValueListField
    Friend WithEvents [hSPROPPAGERANGEID] As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Friend WithEvents [hSPROPPPROJECTCATEGORYCODEID] As Global.Blackbaud.AppFx.UIModeling.Core.CodeTableField
    Friend WithEvents [FORMHEADER] As Global.Blackbaud.AppFx.UIModeling.Core.StringField

    Public Overrides Sub Init()

        Me.MODEL = Me.HostModel
        Me.[FORMTITLE] = Me.HostModel.[FORMTITLE]
        Me.[REVENUECONSTITUENTID] = Me.HostModel.[REVENUECONSTITUENTID]
        Me.[GIFTRECIPIENT] = Me.HostModel.[GIFTRECIPIENT]
        Me.[SPONSORSHIPCONSTITUENTID] = Me.HostModel.[SPONSORSHIPCONSTITUENTID]
        Me.[SPONSORSHIPREASONID] = Me.HostModel.[SPONSORSHIPREASONID]
        Me.[SPONSORSHIPPROGRAMID] = Me.HostModel.[SPONSORSHIPPROGRAMID]
        Me.[PLANNEDENDDATE] = Me.HostModel.[PLANNEDENDDATE]
        Me.[SPONSORSHIPLOCATIONID] = Me.HostModel.[SPONSORSHIPLOCATIONID]
        Me.[SPROPPAGERANGEID] = Me.HostModel.[SPROPPAGERANGEID]
        Me.[GENDERCODE] = Me.HostModel.[GENDERCODE]
        Me.[ISHIVPOSITIVECODE] = Me.HostModel.[ISHIVPOSITIVECODE]
        Me.[HASCONDITIONCODE] = Me.HostModel.[HASCONDITIONCODE]
        Me.[ISORPHANEDCODE] = Me.HostModel.[ISORPHANEDCODE]
        Me.[SPROPPPROJECTCATEGORYCODEID] = Me.HostModel.[SPROPPPROJECTCATEGORYCODEID]
        Me.[SPONSORSHIPOPPORTUNITYIDCHILD] = Me.HostModel.[SPONSORSHIPOPPORTUNITYIDCHILD]
        Me.[SPONSORSHIPOPPORTUNITYIDPROJECT] = Me.HostModel.[SPONSORSHIPOPPORTUNITYIDPROJECT]
        Me.[ISSOLESPONSORSHIP] = Me.HostModel.[ISSOLESPONSORSHIP]
        Me.[STARTDATE] = Me.HostModel.[STARTDATE]
        Me.[AMOUNT] = Me.HostModel.[AMOUNT]
        Me.[AUTOPAY] = Me.HostModel.[AUTOPAY]
        Me.[PAYMENTMETHODCODE] = Me.HostModel.[PAYMENTMETHODCODE]
        Me.[CONSTITUENTACCOUNTID] = Me.HostModel.[CONSTITUENTACCOUNTID]
        Me.[CONSTITUENTACCOUNTIDADDACCOUNTACTION] = Me.HostModel.[CONSTITUENTACCOUNTIDADDACCOUNTACTION]
        Me.[REFERENCENUMBER] = Me.HostModel.[REFERENCENUMBER]
        Me.[REFERENCEDATE] = Me.HostModel.[REFERENCEDATE]
        Me.[FREQUENCYCODE] = Me.HostModel.[FREQUENCYCODE]
        Me.[REVENUESCHEDULESTARTDATE] = Me.HostModel.[REVENUESCHEDULESTARTDATE]
        Me.[REVENUESCHEDULEENDDATE] = Me.HostModel.[REVENUESCHEDULEENDDATE]
        Me.[CREDITCARDNUMBER] = Me.HostModel.[CREDITCARDNUMBER]
        Me.[CARDHOLDERNAME] = Me.HostModel.[CARDHOLDERNAME]
        Me.[EXPIRESON] = Me.HostModel.[EXPIRESON]
        Me.[CREDITCARDTOKEN] = Me.HostModel.[CREDITCARDTOKEN]
        Me.[CREDITTYPECODEID] = Me.HostModel.[CREDITTYPECODEID]
        Me.[RESERVEDOPPORTUNITYIDCHILD] = Me.HostModel.[RESERVEDOPPORTUNITYIDCHILD]
        Me.[RESERVATIONKEY] = Me.HostModel.[RESERVATIONKEY]
        Me.[MATCHEDOPPORTUNITYID] = Me.HostModel.[MATCHEDOPPORTUNITYID]
        Me.[SENDREMINDER] = Me.HostModel.[SENDREMINDER]
        Me.[FINDERNUMBER] = Me.HostModel.[FINDERNUMBER]
        Me.[SOURCECODE] = Me.HostModel.[SOURCECODE]
        Me.[APPEALID] = Me.HostModel.[APPEALID]
        Me.[MAILINGID] = Me.HostModel.[MAILINGID]
        Me.[CHANNELCODEID] = Me.HostModel.[CHANNELCODEID]
        Me.[REFERENCE] = Me.HostModel.[REFERENCE]
        Me.[CATEGORYCODEID] = Me.HostModel.[CATEGORYCODEID]
        Me.[EXPIRATIONREASONID] = Me.HostModel.[EXPIRATIONREASONID]
        Me.[TRANSACTIONCURRENCYID] = Me.HostModel.[TRANSACTIONCURRENCYID]
        Me.[BASEEXCHANGERATEID] = Me.HostModel.[BASEEXCHANGERATEID]
        Me.[EXCHANGERATE] = Me.HostModel.[EXCHANGERATE]
        Me.[BASECURRENCYID] = Me.HostModel.[BASECURRENCYID]
        Me.[BASEAMOUNT] = Me.HostModel.[BASEAMOUNT]
        Me.[BASECURRENCYDECIMALDIGITS] = Me.HostModel.[BASECURRENCYDECIMALDIGITS]
        Me.[BASECURRENCYROUNDINGTYPECODE] = Me.HostModel.[BASECURRENCYROUNDINGTYPECODE]
        Me.[BATCHNUMBER] = Me.HostModel.[BATCHNUMBER]
        Me.[REVENUEDEVELOPMENTFUNCTIONCODEID] = Me.HostModel.[REVENUEDEVELOPMENTFUNCTIONCODEID]
        Me.[DONOTACKNOWLEDGE] = Me.HostModel.[DONOTACKNOWLEDGE]
        Me.[SEPAMANDATEID] = Me.HostModel.[SEPAMANDATEID]
        Me.[SEPAMANDATEIDADDASEPAMANDATEACTION] = Me.HostModel.[SEPAMANDATEIDADDASEPAMANDATEACTION]
        Me.[CARRYFORWARDPASTDUE] = Me.HostModel.[CARRYFORWARDPASTDUE]
        Me.[CARRYFORWARDPASTDUEAMOUNT] = Me.HostModel.[CARRYFORWARDPASTDUEAMOUNT]
        Me.[OTHERPAYMENTMETHODCODEID] = Me.HostModel.[OTHERPAYMENTMETHODCODEID]
        Me.[SELECTOPPORTUNITYID] = Me.HostModel.[SELECTOPPORTUNITYID]
        Me.[ISFIXEDTERMSPONSORSHIP] = Me.HostModel.[ISFIXEDTERMSPONSORSHIP]
        Me.[RGSCHEDULE] = Me.HostModel.[RGSCHEDULE]
        Me.[MOPPORTUNITYNAME] = Me.HostModel.[MOPPORTUNITYNAME]
        Me.[MOPPORTUNITYLOCATION] = Me.HostModel.[MOPPORTUNITYLOCATION]
        Me.[MOPPORTUNITYLOOKUPID] = Me.HostModel.[MOPPORTUNITYLOOKUPID]
        Me.[MOPPORTUNITYGENDER] = Me.HostModel.[MOPPORTUNITYGENDER]
        Me.[MOPPORTUNITYBIRTHDATE] = Me.HostModel.[MOPPORTUNITYBIRTHDATE]
        Me.[MOPPORTUNITYIMAGE] = Me.HostModel.[MOPPORTUNITYIMAGE]
        Me.[FINDERNUMBERSTRING] = Me.HostModel.[FINDERNUMBERSTRING]
        Me.[INVALIDFINDERNUMBERIMAGE] = Me.HostModel.[INVALIDFINDERNUMBERIMAGE]
        Me.[INSTALLMENTS] = Me.HostModel.[INSTALLMENTS]
        Me.[PAYMENTMETHODCODEUI] = Me.HostModel.[PAYMENTMETHODCODEUI]
        Me.[GENERALINFORMATION] = Me.HostModel.[GENERALINFORMATION]
        Me.[SPONSORSHIPOPPORTUNITY] = Me.HostModel.[SPONSORSHIPOPPORTUNITY]
        Me.[PAYMENTSCHEDULE] = Me.HostModel.[PAYMENTSCHEDULE]
        Me.[PAYMENTINFORMATION] = Me.HostModel.[PAYMENTINFORMATION]
        Me.[SPECIALINSTRUCTIONSGROUP] = Me.HostModel.[SPECIALINSTRUCTIONSGROUP]
        Me.[MATCHEDOPPORTUNITY] = Me.HostModel.[MATCHEDOPPORTUNITY]
        Me.[TAB_SPONSORSHIP] = Me.HostModel.[TAB_SPONSORSHIP]
        Me.[TAB_PAYMENT] = Me.HostModel.[TAB_PAYMENT]
        Me.[TAB_EXPIRATION] = Me.HostModel.[TAB_EXPIRATION]
        Me.[FINDOPPORTUNITY] = Me.HostModel.[FINDOPPORTUNITY]
        Me.[SOURCECODELOOKUP] = Me.HostModel.[SOURCECODELOOKUP]
        Me.[CURRENCYACTION] = Me.HostModel.[CURRENCYACTION]
        Me.[hSPONSORSHIPPROGRAMID] = Me.HostModel.[hSPONSORSHIPPROGRAMID]
        Me.[hSPONSORSHIPLOCATIONID] = Me.HostModel.[hSPONSORSHIPLOCATIONID]
        Me.[hGENDERCODE] = Me.HostModel.[hGENDERCODE]
        Me.[hHASCONDITIONCODE] = Me.HostModel.[hHASCONDITIONCODE]
        Me.[hISHIVPOSITIVECODE] = Me.HostModel.[hISHIVPOSITIVECODE]
        Me.[hISORPHANEDCODE] = Me.HostModel.[hISORPHANEDCODE]
        Me.[hSPROPPAGERANGEID] = Me.HostModel.[hSPROPPAGERANGEID]
        Me.[hSPROPPPROJECTCATEGORYCODEID] = Me.HostModel.[hSPROPPPROJECTCATEGORYCODEID]
        Me.[FORMHEADER] = Me.HostModel.[FORMHEADER]

        OnInit()

    End Sub

    Partial Private Sub OnInit()
    End Sub

End Class

