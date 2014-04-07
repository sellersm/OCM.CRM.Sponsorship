Imports Blackbaud.AppFx
Imports Blackbaud.AppFx.UIModeling.Core
Imports MoM.Common


''' <summary>
''' Used for all the Sponsorship Add Forms to validate and handle ui for payment tab
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class AddSponsorshipHelper
	Enum AddSponsorshipFormMode As Byte
		ChildSponsorship
		ProspectChildSponsorship
		UnavilableChildSponsorship
		ProjectSponsorship
	End Enum

	Private Const errorTextCouldNotFindinboundChannelResponseForm As String = "Could not find 'Response Form' Channel code table value."
	Private Const errorTextCouldNotFindCreditCardSchedule As String = "Could not find credit card schedule code table value for - "
	Private Const errorTextCouldNotFindDirectDebitSchedule As String = "Could not find direct debit schedule code table value for - "

	Private Const defaultStartingOnDayOfMonth As Integer = 1				'The day of the month to default the Starting On date to
	Private Const defaultMonthlyChildSponsorshipAmount As Integer = 39		'This should be pulled from the program, but for now just setting to $39 April 2014 Rate Increase pushed rate to $39.

	Private Const frequencyAnnually As Integer = 0
	Private Const frequencySemiAnnually As Integer = 1
	Private Const frequencyQuarterly As Integer = 2
	Private Const frequencyMonthly As Integer = 3

	Private Const paymentMethodeCodeCash As Integer = 0
	Private Const paymentMethodeCodeCheck As Integer = 1
	Private Const paymentMethodeCodeCreditCard As Integer = 2
	Private Const paymentMethodeCodeDirectDebit As Integer = 3

	Private Property model As UIModeling.Core.RootUIModel = Nothing

	Property helperMode As AddSponsorshipFormMode		'Used for specific behavior for view form vs edit form


	' Code Table IDs
	Property inboundChannelResponseForm As String = ""

	Property creditCardScheduleMontly1stWeek As String = ""
	Property creditCardScheduleMontly2ndWeek As String = ""
	Property creditCardScheduleMontly3rdWeek As String = ""
	Property creditCardScheduleMontly4thWeek As String = ""

	Property creditCardScheduleQuarterlyMonth1 As String = ""
	Property creditCardScheduleQuarterlyMonth2 As String = ""
	Property creditCardScheduleQuarterlyMonth3 As String = ""

	Property creditCardScheduleAnnuallyJanuary As String = ""
	Property creditCardScheduleAnnuallyFebruary As String = ""
	Property creditCardScheduleAnnuallyMarch As String = ""
	Property creditCardScheduleAnnuallyApril As String = ""
	Property creditCardScheduleAnnuallyMay As String = ""
	Property creditCardScheduleAnnuallyJune As String = ""
	Property creditCardScheduleAnnuallyJuly As String = ""
	Property creditCardScheduleAnnuallyAugust As String = ""
	Property creditCardScheduleAnnuallySeptember As String = ""
	Property creditCardScheduleAnnuallyOctober As String = ""
	Property creditCardScheduleAnnuallyNovember As String = ""
	Property creditCardScheduleAnnuallyDecember As String = ""


	Property directDebitScheduleMonthly1st As String = ""
	Property directDebitScheduleMonthly20th As String = ""
	Property directDebitScheduleMonthly10th As String = ""

	Property directDebitScheduleQuarterlyMonth1_1st As String = ""
	Property directDebitScheduleQuarterlyMonth1_10th As String = ""
	Property directDebitScheduleQuarterlyMonth1_20th As String = ""
	Property directDebitScheduleQuarterlyMonth2_1st As String = ""
	Property directDebitScheduleQuarterlyMonth2_10th As String = ""
	Property directDebitScheduleQuarterlyMonth2_20th As String = ""
	Property directDebitScheduleQuarterlyMonth3_1st As String = ""
	Property directDebitScheduleQuarterlyMonth3_10th As String = ""
	Property directDebitScheduleQuarterlyMonth3_20th As String = ""


	Public Sub New(ByVal uiModel As UIModeling.Core.RootUIModel, ByVal mode As AddSponsorshipFormMode)
		model = uiModel
		helperMode = mode

		'Add all the handlers
		AddHandler model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueChanged, AddressOf FrequencyCode_ValueChanged
		AddHandler model.Fields(AddSponsorshipFields.AUTOPAY).ValueChanged, AddressOf Autopay_ValueChanged
		AddHandler model.Fields(AddSponsorshipFields.PAYMENTMETHODCODE).ValueChanged, AddressOf PaymentMethodCode_ValueChanged
		AddHandler model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).ValueChanged, AddressOf CreditCardScheduleCodeID_ValueChanged
		AddHandler model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueChanged, AddressOf DirectDebitScheduleCodeID_ValueChanged

	End Sub

	Public Sub InitializeCodeTableVars()
		'Initialize Code Table IDs 
		Using crmSQLConnection = model.GetRequestContext().OpenAppDBConnection()
			inboundChannelResponseForm = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CHANNEL_RESPONSEFORM, True, errorTextCouldNotFindinboundChannelResponseForm)

			creditCardScheduleMontly1stWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY1STWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY1STWEEK)
			creditCardScheduleMontly2ndWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY2NDWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY2NDWEEK)
			creditCardScheduleMontly3rdWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY3RDWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY3RDWEEK)
			creditCardScheduleMontly4thWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY4THWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY4THWEEK)

			creditCardScheduleQuarterlyMonth1 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT)
			creditCardScheduleQuarterlyMonth2 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV)
			creditCardScheduleQuarterlyMonth3 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC)

			creditCardScheduleAnnuallyJanuary = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJANUARY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJANUARY)
			creditCardScheduleAnnuallyFebruary = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYFEBRUARY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYFEBRUARY)
			creditCardScheduleAnnuallyMarch = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMARCH, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMARCH)
			creditCardScheduleAnnuallyApril = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAPRIL, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAPRIL)
			creditCardScheduleAnnuallyMay = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMAY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMAY)
			creditCardScheduleAnnuallyJune = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJUNE, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJUNE)
			creditCardScheduleAnnuallyJuly = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJULY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJULY)
			creditCardScheduleAnnuallyAugust = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAUGUST, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAUGUST)
			creditCardScheduleAnnuallySeptember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYSEPTEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYSEPTEMBER)
			creditCardScheduleAnnuallyOctober = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYOCTOBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYOCTOBER)
			creditCardScheduleAnnuallyNovember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYNOVEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYNOVEMBER)
			creditCardScheduleAnnuallyDecember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYDECEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYDECEMBER)


			directDebitScheduleMonthly1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY1ST)
			directDebitScheduleMonthly10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY10TH)
			directDebitScheduleMonthly20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY20TH)

			directDebitScheduleQuarterlyMonth1_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT1ST)
			directDebitScheduleQuarterlyMonth1_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT10TH)
			directDebitScheduleQuarterlyMonth1_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT20TH)
			directDebitScheduleQuarterlyMonth2_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV1ST)
			directDebitScheduleQuarterlyMonth2_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV10TH)
			directDebitScheduleQuarterlyMonth2_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV20TH)
			directDebitScheduleQuarterlyMonth3_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC1ST)
			directDebitScheduleQuarterlyMonth3_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC10TH)
			directDebitScheduleQuarterlyMonth3_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC20TH)

		End Using
	End Sub

	Public Sub InitializeForm()
		model.Fields(AddSponsorshipFields.AMOUNT).ValueObject = defaultMonthlyChildSponsorshipAmount

		model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(defaultStartingOnDayOfMonth)
		model.Fields(AddSponsorshipFields.CHANNELCODEID).ValueObject = New Guid(inboundChannelResponseForm)
		model.Fields(AddSponsorshipFields.AUTOPAY).ValueObject = False

		SetDisplayForCCDDSchedule()
	End Sub

	' Determine the starting on date based on the day passed in and the current date
	' If the day is greater than or equal to today move it to the next month
	Private Function GetStartingOnDateByDay(Day As Integer) As Date
		Dim startingOnDate As Date

		If Today.Day >= Day Then
			startingOnDate = DateAdd(DateInterval.Month, 1, New Date(Today.Year, Today.Month, Day))
		Else
			startingOnDate = New Date(Today.Year, Today.Month, Day)
		End If

		Return startingOnDate
	End Function


	Private Sub FrequencyCode_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		' Only update the amount based on the frequency if it is a child sponsorship
		If (helperMode = AddSponsorshipFormMode.ChildSponsorship) Or (helperMode = AddSponsorshipFormMode.ProspectChildSponsorship) Or _
		   (helperMode = AddSponsorshipFormMode.UnavilableChildSponsorship) Then

			Select Case e.NewValue
				Case frequencyAnnually
					model.Fields(AddSponsorshipFields.AMOUNT).ValueObject = defaultMonthlyChildSponsorshipAmount * 12

				Case frequencySemiAnnually
					model.Fields(AddSponsorshipFields.AMOUNT).ValueObject = defaultMonthlyChildSponsorshipAmount * 6

				Case frequencyQuarterly
					model.Fields(AddSponsorshipFields.AMOUNT).ValueObject = defaultMonthlyChildSponsorshipAmount * 3

				Case frequencyMonthly
					model.Fields(AddSponsorshipFields.AMOUNT).ValueObject = defaultMonthlyChildSponsorshipAmount
			End Select
		End If
	End Sub

	Private Sub Autopay_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		If Not CBool(model.Fields(AddSponsorshipFields.AUTOPAY).ValueObject) Then
			'Clear the schedule when autopay is unchecked
			model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueObject = Nothing
			model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).ValueObject = Nothing
		End If

		SetDisplayForCCDDSchedule()

	End Sub


	Private Sub PaymentMethodCode_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		' Set the old schedule to nothing
		Select Case e.OldValue
			Case paymentMethodeCodeCreditCard
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).ValueObject = Nothing

			Case paymentMethodeCodeDirectDebit
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueObject = Nothing

		End Select

		SetDisplayForCCDDSchedule()

	End Sub


	' Determine the display and required status for CREDITCARDSCHEDULECODEID & DIRECTDEBITSCHEDULECODEID based on the values of AUTOPAY and PAYMENTMETHODCODE
	Private Sub SetDisplayForCCDDSchedule()
		Dim enabled As Boolean = CBool(model.Fields(AddSponsorshipFields.AUTOPAY).ValueObject)

		Select Case CInt(model.Fields(AddSponsorshipFields.PAYMENTMETHODCODE).ValueObject)
			Case paymentMethodeCodeCreditCard
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Visible = True
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Enabled = enabled
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Required = enabled

				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Visible = False
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Enabled = False
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Required = False

			Case paymentMethodeCodeDirectDebit
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Visible = True
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Enabled = enabled
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Required = enabled

				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Visible = False
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Enabled = False
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Required = False


			Case Else
				' This is in case Blackbaud adds new types
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Visible = False
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Enabled = False
				model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).Visible = False

				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Visible = False
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Enabled = False
				model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).Required = False

		End Select

	End Sub


	' Set Frequcney and Starting On Date based on Credit Card schedule selected
	Private Sub CreditCardScheduleCodeID_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		Select Case model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).ValueObject.ToString
			Case creditCardScheduleMontly1stWeek
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(1)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyMonthly
			Case creditCardScheduleMontly2ndWeek
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(8)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyMonthly
			Case creditCardScheduleMontly3rdWeek
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(15)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyMonthly
			Case creditCardScheduleMontly4thWeek
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(22)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyMonthly

			Case creditCardScheduleQuarterlyMonth1, creditCardScheduleQuarterlyMonth2, creditCardScheduleQuarterlyMonth3
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(defaultStartingOnDayOfMonth)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyQuarterly

			Case creditCardScheduleAnnuallyJanuary, creditCardScheduleAnnuallyFebruary, creditCardScheduleAnnuallyMarch,
			  creditCardScheduleAnnuallyApril, creditCardScheduleAnnuallyMay, creditCardScheduleAnnuallyJune,
			  creditCardScheduleAnnuallyJuly, creditCardScheduleAnnuallyAugust, creditCardScheduleAnnuallySeptember,
			  creditCardScheduleAnnuallyOctober, creditCardScheduleAnnuallyNovember, creditCardScheduleAnnuallyDecember

				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(defaultStartingOnDayOfMonth)
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyAnnually

		End Select
	End Sub


	' Set Frequcney and Starting On Date based on Direct Debit schedule selected
	Private Sub DirectDebitScheduleCodeID_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		Select Case model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueObject.ToString
			Case directDebitScheduleMonthly1st, directDebitScheduleQuarterlyMonth1_1st, directDebitScheduleQuarterlyMonth2_1st, directDebitScheduleQuarterlyMonth3_1st
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(1)

			Case directDebitScheduleMonthly10th, directDebitScheduleQuarterlyMonth1_10th, directDebitScheduleQuarterlyMonth2_10th, directDebitScheduleQuarterlyMonth3_10th
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(10)

			Case directDebitScheduleMonthly20th, directDebitScheduleQuarterlyMonth1_20th, directDebitScheduleQuarterlyMonth2_20th, directDebitScheduleQuarterlyMonth3_20th
				model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject = GetStartingOnDateByDay(20)
		End Select

		Select Case model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueObject.ToString

			Case directDebitScheduleMonthly1st, directDebitScheduleMonthly10th, directDebitScheduleMonthly20th
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyMonthly

			Case directDebitScheduleQuarterlyMonth1_1st, directDebitScheduleQuarterlyMonth1_10th, directDebitScheduleQuarterlyMonth1_20th, _
			 directDebitScheduleQuarterlyMonth2_1st, directDebitScheduleQuarterlyMonth2_10th, directDebitScheduleQuarterlyMonth2_20th, _
			 directDebitScheduleQuarterlyMonth3_1st, directDebitScheduleQuarterlyMonth3_10th, directDebitScheduleQuarterlyMonth3_20th
				model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject = frequencyQuarterly
		End Select
	End Sub

	Public Sub ValidateModel(ByVal Sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValidatedEventArgs)
		Dim valid As Boolean = True
		Dim errorMessage As String = ""

		' Determine the day from the Revenue Schedule Start Date
		Dim startingOnDate As Date = CDate(model.Fields(AddSponsorshipFields.REVENUESCHEDULESTARTDATE).ValueObject)
		Dim startingOnDateDay As Integer = 0
		startingOnDateDay = Day(startingOnDate)

		e.Valid = True


		If (CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyMonthly) And _
		 (CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyQuarterly) And _
		(CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencySemiAnnually) And _
		 (CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyAnnually) Then
			e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
			e.InvalidReason = "You have selected an unsupported credit card Frequency. Please select a Frequency of Monthly, Quarterly, Semi-Annually, or Annually."
			e.Valid = False
		End If


		If CBool(model.Fields(AddSponsorshipFields.AUTOPAY).ValueObject) Then

			Select Case CInt(model.Fields(AddSponsorshipFields.PAYMENTMETHODCODE).ValueObject)
				' Auto Pay 
				Case paymentMethodeCodeCreditCard
					' Credit Card schedule validation
					Select Case model.Fields(AddSponsorshipFields.CREDITCARDSCHEDULECODEID).ValueObject.ToString
						Case creditCardScheduleMontly1stWeek, creditCardScheduleMontly2ndWeek, creditCardScheduleMontly3rdWeek, creditCardScheduleMontly4thWeek
							'- ensure that the date is either the 1st, 8th, 15th or 22nd
							If (startingOnDateDay <> 1) And (startingOnDateDay <> 8) And (startingOnDateDay <> 15) And (startingOnDateDay <> 22) Then
								e.InvalidFieldName = AddSponsorshipFields.REVENUESCHEDULESTARTDATE
								e.InvalidReason = "The starting on date for monthly credit card payments has to be the 1st, 8th, 15th or 22nd of the month."
								e.Valid = False
							ElseIf CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyMonthly Then
								e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
								e.InvalidReason = "You have selected a monthly credit card schedule. However, the Frequency is not Monthly."
								e.Valid = False
							End If

						Case creditCardScheduleQuarterlyMonth1, creditCardScheduleQuarterlyMonth2, creditCardScheduleQuarterlyMonth3
							If (startingOnDateDay <> defaultStartingOnDayOfMonth) Then
								e.InvalidFieldName = AddSponsorshipFields.REVENUESCHEDULESTARTDATE
								e.InvalidReason = "The starting on date for quarterly credit card payments has to be on day " + defaultStartingOnDayOfMonth.ToString
								e.Valid = False
							ElseIf CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyQuarterly Then
								e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
								e.InvalidReason = "You have selected a quarterly credit card schedule. However, the Frequency is not Quarterly."
								e.Valid = False
							End If

						Case creditCardScheduleAnnuallyJanuary, creditCardScheduleAnnuallyFebruary, creditCardScheduleAnnuallyMarch,
						  creditCardScheduleAnnuallyApril, creditCardScheduleAnnuallyMay, creditCardScheduleAnnuallyJune,
						  creditCardScheduleAnnuallyJuly, creditCardScheduleAnnuallyAugust, creditCardScheduleAnnuallySeptember,
						  creditCardScheduleAnnuallyOctober, creditCardScheduleAnnuallyNovember, creditCardScheduleAnnuallyDecember
							If (startingOnDateDay <> defaultStartingOnDayOfMonth) Then
								e.InvalidFieldName = AddSponsorshipFields.REVENUESCHEDULESTARTDATE
								e.InvalidReason = "The starting on date for annually credit card payments has to be on day " + defaultStartingOnDayOfMonth.ToString
								e.Valid = False
							ElseIf CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyAnnually Then
								e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
								e.InvalidReason = "You have selected an annually credit card schedule. However, the Frequency is not Annually."
								e.Valid = False
							End If

					End Select


				Case paymentMethodeCodeDirectDebit
					' Direct Debit schedule validation

					' Direct Debit - 1st, 10th or 20th 
					If (startingOnDateDay <> 1) And (startingOnDateDay <> 10) And (startingOnDateDay <> 20) Then
						e.InvalidFieldName = AddSponsorshipFields.REVENUESCHEDULESTARTDATE
						e.InvalidReason = "The starting on date for direct debit payments has to be the 1st, 10th or 20th of the month."
						e.Valid = False
					Else
						Select Case model.Fields(AddSponsorshipFields.DIRECTDEBITSCHEDULECODEID).ValueObject.ToString
							Case directDebitScheduleMonthly1st, directDebitScheduleMonthly10th, directDebitScheduleMonthly20th
								If CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyMonthly Then
									e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
									e.InvalidReason = "You have selected a monthly direct debit schedule. However, the Frequency is not Monthly."
									e.Valid = False
								End If
							Case directDebitScheduleQuarterlyMonth1_1st, directDebitScheduleQuarterlyMonth1_10th, directDebitScheduleQuarterlyMonth1_20th, _
							 directDebitScheduleQuarterlyMonth2_1st, directDebitScheduleQuarterlyMonth2_10th, directDebitScheduleQuarterlyMonth2_20th, _
							 directDebitScheduleQuarterlyMonth3_1st, directDebitScheduleQuarterlyMonth3_10th, directDebitScheduleQuarterlyMonth3_20th
								If CInt(model.Fields(AddSponsorshipFields.FREQUENCYCODE).ValueObject) <> frequencyQuarterly Then
									e.InvalidFieldName = AddSponsorshipFields.FREQUENCYCODE
									e.InvalidReason = "You have selected a quarterly direct debit schedule. However, the Frequency is not Quarterly."
									e.Valid = False
								End If
						End Select
					End If

			End Select
		Else
			' Cash gift
			If (startingOnDateDay <> defaultStartingOnDayOfMonth) Then
				e.InvalidFieldName = AddSponsorshipFields.REVENUESCHEDULESTARTDATE
				e.InvalidReason = "The starting on date for cash gifts has to be on day " + defaultStartingOnDayOfMonth.ToString
				e.Valid = False

			End If
		End If

	End Sub

End Class
