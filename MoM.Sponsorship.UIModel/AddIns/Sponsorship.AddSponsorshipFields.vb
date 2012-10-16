﻿Public Class AddSponsorshipFields
	Public Const FORMTITLE As String = "FORMTITLE"
	Public Const REVENUECONSTITUENTID As String = "REVENUECONSTITUENTID"
	Public Const INCLUDENONCONSTITUENTRECORDS As String = "INCLUDENONCONSTITUENTRECORDS"
	Public Const FORMHEADER As String = "FORMHEADER"
	Public Const GIFTRECIPIENT As String = "GIFTRECIPIENT"
	Public Const SPONSORSHIPCONSTITUENTID As String = "SPONSORSHIPCONSTITUENTID"
	Public Const SPONSORSHIPREASONID As String = "SPONSORSHIPREASONID"
	Public Const SPONSORSHIPPROGRAMID As String = "SPONSORSHIPPROGRAMID"
	Public Const CHILDREN As String = "CHILDREN"
	Public Const ID As String = "ID"
	Public Const NAME As String = "NAME"
	Public Const LOOKUPID As String = "LOOKUPID"
	Public Const TRANSFERCHILDNAME As String = "TRANSFERCHILDNAME"
	Public Const TRANSFERLOOKUPID As String = "TRANSFERLOOKUPID"
	Public Const TRANSFERCHILDID As String = "TRANSFERCHILDID"
	Public Const PLANNEDENDDATE As String = "PLANNEDENDDATE"
	Public Const SPONSORSHIPLOCATIONID As String = "SPONSORSHIPLOCATIONID"
	Public Const WITHINLOCATION As String = "WITHINLOCATION"
	Public Const WITHINLOCATIONDISPLAY As String = "WITHINLOCATIONDISPLAY"
	Public Const SPROPPAGERANGEID As String = "SPROPPAGERANGEID"
	Public Const GENDERCODE As String = "GENDERCODE"
	Public Const ISHIVPOSITIVECODE As String = "ISHIVPOSITIVECODE"
	Public Const HASCONDITIONCODE As String = "HASCONDITIONCODE"
	Public Const ISORPHANEDCODE As String = "ISORPHANEDCODE"
	Public Const SPROPPPROJECTCATEGORYCODEID As String = "SPROPPPROJECTCATEGORYCODEID"
	Public Const SUBSTITUTECHILDID As String = "SUBSTITUTECHILDID"
	Public Const ELIGIBILITYCODE As String = "ELIGIBILITYCODE"
	Public Const AVAILABILITYCODE As String = "AVAILABILITYCODE"
	Public Const RESTRICTFORSOLESPONSORSHIP As String = "RESTRICTFORSOLESPONSORSHIP"
	Public Const SPONSORSHIPOPPORTUNITYGROUPID As String = "SPONSORSHIPOPPORTUNITYGROUPID"
	Public Const RESERVATIONKEYID As String = "RESERVATIONKEYID"
	Public Const CORRESPONDINGSPONSORID As String = "CORRESPONDINGSPONSORID"
	Public Const FINANCIALSPONSORID As String = "FINANCIALSPONSORID"
	Public Const SPONSORSHIPOPPORTUNITYIDCHILD As String = "SPONSORSHIPOPPORTUNITYIDCHILD"
	Public Const SPONSORSHIPOPPORTUNITYIDPROJECT As String = "SPONSORSHIPOPPORTUNITYIDPROJECT"
	Public Const ISSOLESPONSORSHIP As String = "ISSOLESPONSORSHIP"
	Public Const STARTDATE As String = "STARTDATE"
	Public Const AMOUNT As String = "AMOUNT"
	Public Const AUTOPAY As String = "AUTOPAY"
	Public Const PAYMENTMETHODCODE As String = "PAYMENTMETHODCODE"
	Public Const CONSTITUENTACCOUNTID As String = "CONSTITUENTACCOUNTID"
	Public Const REFERENCENUMBER As String = "REFERENCENUMBER"
	Public Const REFERENCEDATE As String = "REFERENCEDATE"
	Public Const FREQUENCYCODE As String = "FREQUENCYCODE"
	Public Const REVENUESCHEDULESTARTDATE As String = "REVENUESCHEDULESTARTDATE"
	Public Const REVENUESCHEDULEENDDATE As String = "REVENUESCHEDULEENDDATE"
	Public Const CREDITCARDNUMBER As String = "CREDITCARDNUMBER"
	Public Const CARDHOLDERNAME As String = "CARDHOLDERNAME"
	Public Const EXPIRESON As String = "EXPIRESON"
	Public Const CREDITCARDTOKEN As String = "CREDITCARDTOKEN"
	Public Const CREDITTYPECODEID As String = "CREDITTYPECODEID"
	Public Const RESERVEDOPPORTUNITYIDCHILD As String = "RESERVEDOPPORTUNITYIDCHILD"
	Public Const RESERVATIONKEY As String = "RESERVATIONKEY"
	Public Const INCLUDEINACTIVE As String = "INCLUDEINACTIVE"
	Public Const MATCHEDOPPORTUNITYID As String = "MATCHEDOPPORTUNITYID"
	Public Const SENDREMINDER As String = "SENDREMINDER"
	Public Const FINDERNUMBER As String = "FINDERNUMBER"
	Public Const SOURCECODE As String = "SOURCECODE"
	Public Const APPEALID As String = "APPEALID"
	Public Const MAILINGID As String = "MAILINGID"
	Public Const STATUS As String = "STATUS"
	Public Const INCLUDEAPPEALMAILINGS As String = "INCLUDEAPPEALMAILINGS"
	Public Const CHANNELCODEID As String = "CHANNELCODEID"
	Public Const REFERENCE As String = "REFERENCE"
	Public Const CATEGORYCODEID As String = "CATEGORYCODEID"
	Public Const EXPIRATIONREASONID As String = "EXPIRATIONREASONID"
	Public Const TRANSACTIONCURRENCYID As String = "TRANSACTIONCURRENCYID"
	Public Const BASEEXCHANGERATEID As String = "BASEEXCHANGERATEID"
	Public Const EXCHANGERATE As String = "EXCHANGERATE"
	Public Const BASECURRENCYID As String = "BASECURRENCYID"
	Public Const BASEAMOUNT As String = "BASEAMOUNT"
	Public Const BASECURRENCYDECIMALDIGITS As String = "BASECURRENCYDECIMALDIGITS"
	Public Const BASECURRENCYROUNDINGTYPECODE As String = "BASECURRENCYROUNDINGTYPECODE"
	Public Const BATCHNUMBER As String = "BATCHNUMBER"
	Public Const REVENUEDEVELOPMENTFUNCTIONCODEID As String = "REVENUEDEVELOPMENTFUNCTIONCODEID"
	Public Const DONOTACKNOWLEDGE As String = "DONOTACKNOWLEDGE"
	Public Const PFMID As String = "PFMID"
	Public Const INTERACTIONTYPECODEID As String = "INTERACTIONTYPECODEID"
	Public Const FUNDRAISERID As String = "FUNDRAISERID"
	Public Const DONORCONTACTCODEID As String = "DONORCONTACTCODEID"
	Public Const ISPROSPECT As String = "ISPROSPECT"
	Public Const SELECTOPPORTUNITYID As String = "SELECTOPPORTUNITYID"
	Public Const ISFIXEDTERMSPONSORSHIP As String = "ISFIXEDTERMSPONSORSHIP"
	Public Const RGSCHEDULE As String = "RGSCHEDULE"
	Public Const MOPPORTUNITYNAME As String = "MOPPORTUNITYNAME"
	Public Const MOPPORTUNITYLOCATION As String = "MOPPORTUNITYLOCATION"
	Public Const MOPPORTUNITYLOOKUPID As String = "MOPPORTUNITYLOOKUPID"
	Public Const MOPPORTUNITYGENDER As String = "MOPPORTUNITYGENDER"
	Public Const MOPPORTUNITYBIRTHDATE As String = "MOPPORTUNITYBIRTHDATE"
	Public Const MOPPORTUNITYIMAGE As String = "MOPPORTUNITYIMAGE"
	Public Const FINDERNUMBERSTRING As String = "FINDERNUMBERSTRING"
	Public Const INVALIDFINDERNUMBERIMAGE As String = "INVALIDFINDERNUMBERIMAGE"
	Public Const ISPROSPECTSPONSORSHIP As String = "ISPROSPECTSPONSORSHIP"
	Public Const INSTALLMENTS As String = "INSTALLMENTS"
	Public Const DATEFIELD As String = "DATE" 'DATE is a reserved word and could not be the name of the constant
	Public Const CREDITCARDSCHEDULECODEID As String = "CREDITCARDSCHEDULECODEID"
	Public Const DIRECTDEBITSCHEDULECODEID As String = "DIRECTDEBITSCHEDULECODEID"
End Class
