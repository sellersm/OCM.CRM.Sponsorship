Imports Blackbaud.AppFx.UIModeling.Core.Utilities
Imports Blackbaud.AppFx.Server
Imports Blackbaud.AppFx.XmlTypes
Imports Blackbaud.AppFx.XmlTypes.DataForms
Imports Blackbaud.AppFx.Fundraising.UIModel
Imports System.Globalization
Imports Blackbaud.AppFx.Sponsorship.UIModel

Public Class SponsorshipAddFormWrappedUIModel
    Implements IHasOpportunityProgram

    Private Const BUTTONLABEL_MATCH As String = "Find"
    Private Const BUTTONLABEL_CLEAR As String = "Clear"

    Private Const GROUPINFO As String = "298a1b75-b1e4-4be6-b4af-dc5d1633f436"
    Private Const PROGRAMINFO As String = "26c9edcb-a40f-4cf3-9cd4-fb01f773a777"
    Private Const AGERANGEDATALIST As String = "b2b6fb02-19df-49d4-8092-de9611e98b2a"

    Private Const TRANSFERFORMID As String = "c5012f3e-a9b4-4d0d-bc27-6fa3818c71c2"
    Private Const OVERRIDETRANSFERFORMID As String = "9bc8daf7-4f5f-4411-a8fd-64838e7d874f"
    Private Const REASSIGNFORMID As String = "421a8ead-9eab-4fa8-8219-760fb3726e95"

    Private Enum CHANGEOPPORTUNITY
        YES
        FINANCIAL_SPONSOR
        CORRESPONDING_SPONSOR
        GIFTRECIPIENT
        PROGRAM
    End Enum

    Private Enum CURRENTFORM
        ADD
        TRANSFER
        REASSIGN
    End Enum

#Region "Private Variables"

    Private ReadOnly INVALIDOPP As String = "The currently selected opportunity is not valid for the selected {0}.  Continuing with this change will require selection of a new opportunity.  Do you wish to continue?"

    Private _initialLoadComplete As Boolean = False
    Private _changeEventFired As Boolean = False
    Private _displayConfirm As Boolean = False

    Private _sponsorshipConstituent As String
    Private _sponsorshipConstituentName As String
    Private _revenueConstituent As String
    Private _revenueConstituentName As String
    Private _lookupid As String

    Private _programValue As Guid
    '(case [SPONSORSHIPOPPORTUNITYTYPECODE] when (1) then N'Child' when (2) then N'Project'  end)
    Private Const CHILD_OPPORTUNITY As Integer = 1
    Private Const PROJECT_OPPORTUNITY As Integer = 2
    Private _opportunitytype As Integer  ' holds value to determine if this is a child (1) sponsorship or project (2) sponsorship
    Private _processingSoleSponsorship As Boolean = False
    Private _cacheSoleSponsorship As Boolean = False
    Private _groupId As String
    Private _previousGroupId As String = Nothing
    Private _requireLocationId As Boolean = False
    'Private _previousLocationId As Guid
    'Private _currentLocationID As String = Nothing

    Private _soleSponsorshipOverrides As Boolean = False
    'Private _lockOverrides As Boolean = False
    Private _amountSet As Boolean = False
    Private _isAffiliate As Boolean

    Private _securityContext As New RequestSecurityContext

    Private lockHelper As SponsorshipOpportunityLockHelperUIModel
    Private _groupLockedFields As New List(Of Object)

    Private _changedOpportunity As Nullable(Of CHANGEOPPORTUNITY) = Nothing

    Private _finderNumberHelper As FinderNumberHelper
    Private _spotRateID As Guid = New Guid("00000000-0000-0000-0000-000000000001")
    Private _currentForm As CURRENTFORM = CURRENTFORM.ADD
    Private _tempBatchNumber As UIModeling.Core.StringField = Nothing
    Private _programHelper As SponsorshipGroupRestrictionsHelper
    Private _cacheSourceCode As String
    Private _cacheAppealId As Guid
    Private _cacheMailingId As Guid
#End Region


#Region "Helper Interface"

    ReadOnly Property hSPONSORSHIPPROGRAMID As UIModeling.Core.SimpleDataListField(Of System.Guid) Implements IHasOpportunityProgram.hSPONSORSHIPPROGRAMID
        Get
            Return SPONSORSHIPPROGRAMID
        End Get
    End Property

    ReadOnly Property hSPONSORSHIPLOCATIONID As UIModeling.Core.SearchListField(Of System.Guid) Implements IHasOpportunityProgram.hSPONSORSHIPLOCATIONID
        Get
            Return SPONSORSHIPLOCATIONID
        End Get
    End Property

    ReadOnly Property hGENDERCODE As UIModeling.Core.ValueListField Implements IHasOpportunityProgram.hGENDERCODE
        Get
            Return GENDERCODE
        End Get
    End Property

    ReadOnly Property hHASCONDITIONCODE As UIModeling.Core.ValueListField Implements IHasOpportunityProgram.hHASCONDITIONCODE
        Get
            Return HASCONDITIONCODE
        End Get
    End Property

    ReadOnly Property hISHIVPOSITIVECODE As UIModeling.Core.ValueListField Implements IHasOpportunityProgram.hISHIVPOSITIVECODE
        Get
            Return ISHIVPOSITIVECODE
        End Get
    End Property

    ReadOnly Property hISORPHANEDCODE As UIModeling.Core.ValueListField Implements IHasOpportunityProgram.hISORPHANEDCODE
        Get
            Return ISORPHANEDCODE
        End Get
    End Property

    ReadOnly Property hSPROPPAGERANGEID As UIModeling.Core.SimpleDataListField(Of System.Guid) Implements IHasOpportunityProgram.hSPROPPAGERANGEID
        Get
            Return SPROPPAGERANGEID
        End Get
    End Property

    ReadOnly Property hSPROPPPROJECTCATEGORYCODEID As UIModeling.Core.CodeTableField Implements IHasOpportunityProgram.hSPROPPPROJECTCATEGORYCODEID
        Get
            Return SPROPPPROJECTCATEGORYCODEID
        End Get
    End Property

#End Region

    Private Sub SponsorshipAddFormUIModel_Loaded(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
        lockHelper = New SponsorshipOpportunityLockHelperUIModel(_securityContext)
        _finderNumberHelper = New FinderNumberHelper(New FinderNumberHelperArgs(Me.RootUIModel, Me.FINDERNUMBERSTRING, Me.FINDERNUMBER, Me.REVENUECONSTITUENTID, Me.APPEALID, Me.MAILINGID, Me.SOURCECODE, Me.SOURCECODELOOKUP, Me.INVALIDFINDERNUMBERIMAGE))
        _programHelper = New SponsorshipGroupRestrictionsHelper(Me)

        If Me.DataFormInstanceId.Equals(New Guid(REASSIGNFORMID)) Then
            _currentForm = CURRENTFORM.REASSIGN
        ElseIf Me.DataFormInstanceId.Equals(New Guid(TRANSFERFORMID)) OrElse Me.DataFormInstanceId.Equals(New Guid(OVERRIDETRANSFERFORMID)) Then
            _currentForm = CURRENTFORM.TRANSFER
        End If

        _initialLoadComplete = True

        AddHandler Me.CHILDREN.Value.RemovingItem, AddressOf ChildrenSelected_ListChanged
        AddHandler Me.CHILDREN.Value.ListChanged, AddressOf ChildrenSelected_AddingNewRow
        'AddHandler Me.CHILDREN.Value., AddressOf ChildrenSelected_ChildInserted
        'AddHandler Me.CHILDREN.Value.Item(0).NAMESEARCHACTION.SearchItemSelected, AddressOf ChildrenSelected_AddedNew

        SetupInitialValues()

        Me.SPONSORSHIPOPPORTUNITYIDCHILD.Enabled = True

    End Sub

    Private Sub SetupInitialValues()
        'Sponsorship panel fields
        Me.SPONSORSHIPCONSTITUENTID.Enabled = False
        Me.TAB_PAYMENT.Enabled = False

        'set the program field to "Individual Child Sponsorship"
        Me.SPONSORSHIPPROGRAMID.SetValueFromLabel("Individual Child Sponsorship")

        'disable the Remove button until a child is selected
        Me.REMOVESELECTEDCHILD.Enabled = False

        If Not Me.REVENUESCHEDULESTARTDATE.HasValue Then
            Me.REVENUESCHEDULESTARTDATE.Value = Date.Today.Date()
        End If
        If Not Me.STARTDATE.HasValue Then
            Me.STARTDATE.Value = Date.Today.Date()
        End If

        'handle reassign and transfer
        If _currentForm = CURRENTFORM.REASSIGN Then
            Me.SPONSORSHIPPROGRAMID.Enabled = False
            SetFieldsForTransferandReassign("9")
            Me.REVENUECONSTITUENTID.Value = Nothing
            Me.REVENUECONSTITUENTID.SearchDisplayText = String.Empty
            Me.GIFTRECIPIENT.Value = False
            'before clearing out the findernumber cache the effort fields
            _cacheSourceCode = Me.SOURCECODE.Value
            _cacheAppealId = Me.APPEALID.Value
            _cacheMailingId = Me.MAILINGID.Value
            Me.FINDERNUMBERSTRING.Value = String.Empty
            'reset the effort fields
            Me.SOURCECODE.Value = _cacheSourceCode
            Me.APPEALID.Value = _cacheAppealId
            Me.APPEALID.UpdateDisplayText(_cacheAppealId)
            Me.MAILINGID.Value = _cacheMailingId
            Me.MAILINGID.UpdateDisplayText(_cacheMailingId)
        ElseIf _currentForm = CURRENTFORM.TRANSFER Then
            Me.ISSOLESPONSORSHIP.Enabled = False
            Me.REVENUECONSTITUENTID.Enabled = False
            Me.REVENUEDEVELOPMENTFUNCTIONCODEID.Enabled = True
            Me.REVENUEDEVELOPMENTFUNCTIONCODEID.Visible = True
            Me.Fields.Remove(_batchnumber)
            'reason has to be used with right parameter
            SetFieldsForTransferandReassign("5")
        Else
            Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Visible = False
        End If

        If Not Me.SPONSORSHIPPROGRAMID.Value.Equals(Guid.Empty) Then
            _cacheSoleSponsorship = Me.ISSOLESPONSORSHIP.Value
            'Prevent overwriting of amount.
            _amountSet = True
            HandleSponsorshipProgram(False, False)
            SaveKeyValues()
            Me.TAB_PAYMENT.Enabled = True
        End If

        If Not _currentForm = CURRENTFORM.TRANSFER Then
            'matching opportunity fields
            ' leave the matching section enabled, no matter what:
            'OpportunitySelectionEnableDisable(False)
            Me.ISSOLESPONSORSHIP.Visible = False
            ' leave the children list enabled no matter what:
            'ChildrenListEnableDisable(False)
        End If

        'matched opportunity panel
        MatchedOpportunityVisible(False)

        'fixed term fields
        If Me.EXPIRATIONREASONID.Value.Equals(Guid.Empty) Then
            Me.EXPIRATIONREASONID.Enabled = False
            Me.PLANNEDENDDATE.Enabled = False
        Else
            Me.ISFIXEDTERMSPONSORSHIP.Value = True
            Me.EXPIRATIONREASONID.Enabled = True
            Me.PLANNEDENDDATE.Enabled = True
        End If

        'revenue
        Me.REVENUESCHEDULEENDDATE.Enabled = False

        'make installments grid readonly
        Me.INSTALLMENTS.DisplayReadOnly = True
        Me.INSTALLMENTS.AllowAdd = False
        Me.INSTALLMENTS.AllowDelete = False

        UpdatePaymentFieldsEnabled()
        UpdatePaymentFieldsVisible()
        EnableFieldsForConstituent()
        UpdatePaymentFieldsRequired()
        'UpdateConstituentSearchAddForms() 'Do we need this?

        If ConditionSettingHelper.ConditionSettingExists("Multicurrency", Me.GetRequestContext()) AndAlso Not _isAffiliate Then
            Me.TRANSACTIONCURRENCYID.Required = True
            Me.TRANSACTIONCURRENCYID.Value = Me.BASECURRENCYID.Value
            Me.BASEEXCHANGERATEID.Value = Nothing
            Me.EXCHANGERATE.Value = 1D
        Else
            Me.CURRENCYACTION.Visible = False
            Me.BASEAMOUNT.Visible = False
        End If

        _amountSet = True

        'turn off the Add Child button, until user selects a child to sponsor:
        Me.ADDSELECTEDCHILD.Enabled = False
        _lookupid = String.Empty

    End Sub

    Private Sub SetFieldsForTransferandReassign(ByVal reasontype As String)

        Me.STARTDATE.Enabled = False

        'reason has to be used with right parameter
        Me.Fields.Remove(_sponsorshipreasonid)
        Me.SPONSORSHIPREASONID.Visible = True
        Me.SPONSORSHIPREASONID.Enabled = True
        Me.SPONSORSHIPREASONID.DBReadOnly = False
        _sponsorshipreasonid.Parameters.Add(New Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListParameter("TYPE", reasontype))
        Me.Fields.Add(_sponsorshipreasonid)

        LoadInstallments()
    End Sub

    Private Sub _isfixedtermsponsorship_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _isfixedtermsponsorship.ValueChanged
        Me.PLANNEDENDDATE.Enabled = Me.ISFIXEDTERMSPONSORSHIP.Value
        Me.EXPIRATIONREASONID.Enabled = Me.ISFIXEDTERMSPONSORSHIP.Value
        Me.PLANNEDENDDATE.Required = Me.ISFIXEDTERMSPONSORSHIP.Value
        Me.EXPIRATIONREASONID.Required = Me.ISFIXEDTERMSPONSORSHIP.Value
        If Not Me.ISFIXEDTERMSPONSORSHIP.Value Then
            Me.PLANNEDENDDATE.Value = Nothing
            Me.EXPIRATIONREASONID.Value = Nothing
        End If
    End Sub

    Private Sub HandleChangeFinancialSponsor()
        If Me.GIFTRECIPIENT.Value AndAlso Me.REVENUECONSTITUENTID.Value = Me.SPONSORSHIPCONSTITUENTID.Value And Not Me.REVENUECONSTITUENTID.Value.Equals(Guid.Empty) Then
            'User selected same financial sponsor and corresponding sponsor - this is not a gift sponsorship.
            Me.SPONSORSHIPCONSTITUENTID.Enabled = False
            Me.GIFTRECIPIENT.Value = False
        End If
        If ReadyForOpportunity() Then
            HandleSponsorshipProgram(True, False)
            If MatchValid() Then
                MatchOpportunity()
            End If
            SaveKeyValues()
        Else
            OpportunitySelectionEnableDisable(False)
        End If
    End Sub

    Private Sub _revenueconstituentid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _revenueconstituentid.ValueChanged
        If _initialLoadComplete AndAlso Not _changeEventFired Then
            _changeEventFired = True
            If Not Me.GIFTRECIPIENT.Value Then
                Me.SPONSORSHIPCONSTITUENTID.Value = Me.REVENUECONSTITUENTID.Value
                Me.SPONSORSHIPCONSTITUENTID.UpdateDisplayText(Me.REVENUECONSTITUENTID.Value)
            End If
            Me.CONSTITUENTACCOUNTID.Value = Nothing
            Me.CONSTITUENTACCOUNTID.ResetDataSource()
            'placeholder for when changing opportunity
            If CheckSelectedOpportunity("financial sponsor") = CHANGEOPPORTUNITY.YES Then
                HandleChangeFinancialSponsor()
            End If
            _changeEventFired = False
        End If
    End Sub
    Private Sub HandleChangeCorrespondingSponsor()
        If Me.REVENUECONSTITUENTID.Value = Me.SPONSORSHIPCONSTITUENTID.Value And Not Me.SPONSORSHIPCONSTITUENTID.Value.Equals(Guid.Empty) Then
            'User selected same financial sponsor and corresponding sponsor - this is not a gift sponsorship.
            SPONSORSHIPCONSTITUENTID.Enabled = False
            Me.GIFTRECIPIENT.Value = False
        End If
        If ReadyForOpportunity() Then
            HandleSponsorshipProgram(True, False)
            If MatchValid() Then
                MatchOpportunity()
            End If
            SaveKeyValues()
        Else
            OpportunitySelectionEnableDisable(False)
        End If
    End Sub

    Private Sub _sponsorshipconstituentid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _sponsorshipconstituentid.ValueChanged
        If _initialLoadComplete AndAlso Not _changeEventFired Then
            _changeEventFired = True
            If CheckSelectedOpportunity("corresponding sponsor") = CHANGEOPPORTUNITY.YES Then
                HandleChangeCorrespondingSponsor()
            End If
            _changeEventFired = False
        End If
    End Sub

    Private Sub _giftrecipient_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _giftrecipient.ValueChanged
        If _initialLoadComplete AndAlso Not _changeEventFired Then
            _changeEventFired = True
            If Me.GIFTRECIPIENT.Value Then
                Me.SPONSORSHIPCONSTITUENTID.Enabled = True
                Me.SPONSORSHIPCONSTITUENTID.Value = Nothing
                Me.SPONSORSHIPCONSTITUENTID.UpdateDisplayText()
                OpportunitySelectionEnableDisable(False)
            Else
                Me.SPONSORSHIPCONSTITUENTID.Enabled = False
                Me.SPONSORSHIPCONSTITUENTID.Value = Me.REVENUECONSTITUENTID.Value
                Me.SPONSORSHIPCONSTITUENTID.UpdateDisplayText(Me.REVENUECONSTITUENTID.Value)
                If CheckSelectedOpportunity("gift recipient") = CHANGEOPPORTUNITY.YES Then
                    HandleChangeGiftRecipient()
                End If
            End If
            _changeEventFired = False
        End If
    End Sub
    Private Sub HandleChangeGiftRecipient()
        If ReadyForOpportunity() Then
            HandleSponsorshipProgram(True, False)
            If MatchValid() Then
                MatchOpportunity()
            ElseIf Not Me.MATCHEDOPPORTUNITYID.Value.Equals(Guid.Empty) Then
                ' commented by Memphis so the greatest need search isn't ever disabled:
                'Me.SPONSORSHIPLOCATIONID.Enabled = False
                'Me.SPROPPAGERANGEID.Enabled = False
                'Me.GENDERCODE.Enabled = False
                'Me.ISORPHANEDCODE.Enabled = False
                'Me.ISHIVPOSITIVECODE.Enabled = False
                'Me.HASCONDITIONCODE.Enabled = False
                'Me.SPROPPPROJECTCATEGORYCODEID.Enabled = False
            End If
            SaveKeyValues()
        End If
    End Sub

    Private Sub _sponsorshipprogramid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _sponsorshipprogramid.ValueChanged
        ' commented by Memphis so the greatest need search isn't ever disabled:
        'Me.FINDOPPORTUNITY.Enabled = False
        If Not _displayConfirm AndAlso Me.SPONSORSHIPPROGRAMID.HasValue Then
            HandleChangeProgram()
        End If
    End Sub

    Private Sub HandleChangeProgramOpportunity()
        HandleSponsorshipProgram(True, True)
        If MatchValid() Then
            MatchOpportunity()
        End If
        SaveKeyValues()
    End Sub

    Private Sub HandleChangeProgram()
        If _initialLoadComplete Then
            If ReadyForOpportunity() Then
                If _programValue <> Me.SPONSORSHIPPROGRAMID.Value Then
                    If CheckSelectedOpportunity("program") = CHANGEOPPORTUNITY.YES Then
                        HandleChangeProgramOpportunity()
                    End If
                End If
                Me.TAB_PAYMENT.Enabled = True
            End If
            EnableFieldsForConstituent()
        End If
    End Sub
    Private Function ReadyForOpportunity() As Boolean
        Return _initialLoadComplete AndAlso _
               Not String.IsNullOrEmpty(Me.REVENUECONSTITUENTID.SearchDisplayText) AndAlso _
               Not String.IsNullOrEmpty(Me.SPONSORSHIPCONSTITUENTID.SearchDisplayText) AndAlso _
               Me.SPONSORSHIPPROGRAMID.Enabled AndAlso _
               Not Me.SPONSORSHIPPROGRAMID.Value.Equals(Guid.Empty)
    End Function

    Private Sub SaveKeyValues()
        _sponsorshipConstituent = Me.SPONSORSHIPCONSTITUENTID.Value.ToString
        _sponsorshipConstituentName = Me.SPONSORSHIPCONSTITUENTID.SearchDisplayText
        _revenueConstituent = Me.REVENUECONSTITUENTID.Value.ToString
        _revenueConstituentName = Me.REVENUECONSTITUENTID.SearchDisplayText
        _programValue = Me.SPONSORSHIPPROGRAMID.Value
    End Sub

    Private Function CheckSelectedOpportunity(ByVal changedFieldText As String) As CHANGEOPPORTUNITY
        Dim oppy As String = GetOpportunity()
        If Not String.IsNullOrEmpty(oppy) AndAlso Not New Guid(oppy).Equals(Guid.Empty) Then
            If ReadyForOpportunity() Then
                If Not RevalidateOpportunity(oppy) Then
                    Dim msg As String

                    msg = String.Format(CultureInfo.CurrentCulture, INVALIDOPP, IIf(changedFieldText.Equals("gift recipient"), "corresponding sponsor", changedFieldText))

                    _displayConfirm = True
                    Me.Prompts.Add(New UIPrompt() With { _
                                .Text = msg, _
                                .ButtonStyle = UIPromptButtonStyle.YesNo, _
                                .Callback = AddressOf PromptCallBack})
                    _displayConfirm = False
                    If changedFieldText.Equals("financial sponsor") Then
                        _changedOpportunity = CHANGEOPPORTUNITY.FINANCIAL_SPONSOR
                        Return CHANGEOPPORTUNITY.FINANCIAL_SPONSOR
                    ElseIf changedFieldText.Equals("corresponding sponsor") Then
                        _changedOpportunity = CHANGEOPPORTUNITY.CORRESPONDING_SPONSOR
                        Return CHANGEOPPORTUNITY.CORRESPONDING_SPONSOR
                    ElseIf changedFieldText.Equals("program") Then
                        _changedOpportunity = CHANGEOPPORTUNITY.PROGRAM
                        Return CHANGEOPPORTUNITY.PROGRAM
                    Else
                        _changedOpportunity = CHANGEOPPORTUNITY.GIFTRECIPIENT
                        Return CHANGEOPPORTUNITY.GIFTRECIPIENT
                    End If
                End If
            End If
        End If
        Return CHANGEOPPORTUNITY.YES
    End Function

    Private Function GetOpportunity() As String
        If SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity Then
            If Not String.IsNullOrEmpty(Me.SPONSORSHIPOPPORTUNITYIDCHILD.SearchDisplayText) Then
                Return Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value.ToString
            Else
                Return Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Value.ToString
            End If
        ElseIf SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseAReservedOpportunity Then
            Return Me.RESERVEDOPPORTUNITYIDCHILD.Value.ToString
        ElseIf SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed Then
            Return Me.MATCHEDOPPORTUNITYID.Value.ToString
        End If
        Return Nothing
    End Function

    Private Function RevalidateOpportunity(ByVal opportunityId As String) As Boolean
        Dim request As New SearchListLoadRequest

        request.SearchListID = New Guid(CStr(IIf(_opportunitytype = CHILD_OPPORTUNITY, "ab076868-114a-4696-afe9-8d590677708c", "b095dc74-4c09-40c9-9c9d-e8ff55b584ce")))
        request.SecurityContext = _securityContext
        request.Filter = New DataFormItem
        With request.Filter
            .SetValue("SPONSORSHIPOPPORTUNITYID", opportunityId)
            .SetValue("SPONSORSHIPPROGRAMID", Me.SPONSORSHIPPROGRAMID.Value)
            .SetValue("CORRESPONDINGSPONSORID", Me.SPONSORSHIPCONSTITUENTID.Value)
            .SetValue("FINANCIALSPONSORID", Me.REVENUECONSTITUENTID.Value)
        End With
        Dim reply As SearchListLoadReply
        reply = SearchListLoad(request, Me.GetRequestContext())

        Return reply.Output.RowCount > 0
    End Function

    Private Function MatchValid() As Boolean
        Return SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed AndAlso FINDOPPORTUNITY.Caption = BUTTONLABEL_MATCH
    End Function

    Private Sub MatchOpportunity()
        Dim request As New DataListLoadRequest
        'use the MoM Custom datalist:
        request.DataListID = New Guid("4bb7d5f4-d8ad-464b-8ab5-56febd1a95c3") 'New Guid("156c6b80-a895-411f-bebb-198c7a2f9874")
        request.SecurityContext = _securityContext
        request.Parameters = New DataFormItem
        With request.Parameters
            .SetValue("SPONSORSHIPPROGRAMID", Me.SPONSORSHIPPROGRAMID.Value)
            .SetValue("SPONSORSHIPLOCATIONID", IIf(Me.SPONSORSHIPLOCATIONID.Value.Equals(Guid.Empty), Nothing, Me.SPONSORSHIPLOCATIONID.Value))
            If Me.GENDERCODE.HasValue Then
                .SetValue("GENDERCODE", CInt(Me.GENDERCODE.Value))
            End If
            .SetValue("SPROPPAGERANGEID", IIf(Me.SPROPPAGERANGEID.Value.Equals(Guid.Empty), Nothing, Me.SPROPPAGERANGEID.Value))
            If Me.ISHIVPOSITIVECODE.HasValue Then
                .SetValue("ISHIVPOSITIVECODE", CInt(Me.ISHIVPOSITIVECODE.Value))
            End If
            If Me.HASCONDITIONCODE.HasValue Then
                .SetValue("HASCONDITIONCODE", CInt(Me.HASCONDITIONCODE.Value))
            End If
            If Me.ISORPHANEDCODE.HasValue Then
                .SetValue("ISORPHANEDCODE", CInt(Me.ISORPHANEDCODE.Value))
            End If
            .SetValue("SPROPPPROJECTCATEGORYCODEID", IIf(Me.SPROPPPROJECTCATEGORYCODEID.Value.Equals(Guid.Empty), Nothing, Me.SPROPPPROJECTCATEGORYCODEID.Value))
            .SetValue("SPONSORID", IIf(Me.SPONSORSHIPCONSTITUENTID.Value.Equals(Guid.Empty), Nothing, Me.SPONSORSHIPCONSTITUENTID.Value))
            .SetValue("ISSOLESPONSORSHIP", Me.ISSOLESPONSORSHIP.Value)
            .SetValue("REVENUECONSTITUENTID", IIf(Me.REVENUECONSTITUENTID.Value.Equals(Guid.Empty), Nothing, Me.REVENUECONSTITUENTID.Value))
        End With
        Dim reply As DataListLoadReply
        reply = DataListLoad(request, Me.GetRequestContext)

        If Not String.IsNullOrEmpty(reply.Rows(0).Values(0)) Then
            Me.MATCHEDOPPORTUNITYID.Value = New Guid(reply.Rows(0).Values(0))
        End If

        If Not Me.MATCHEDOPPORTUNITYID.Value.Equals(Guid.Empty) Then
            Me.MOPPORTUNITYNAME.Value = reply.Rows(0).Values(1)
            Me.MOPPORTUNITYGENDER.Value = reply.Rows(0).Values(3)
            Me.MOPPORTUNITYLOOKUPID.Value = reply.Rows(0).Values(5)
            Me.MOPPORTUNITYLOCATION.Value = reply.Rows(0).Values(6)

            MatchedOpportunityVisible(True)

            If _opportunitytype = CHILD_OPPORTUNITY Then
                Me.MOPPORTUNITYBIRTHDATE.Value = FuzzyDate.Parse(reply.Rows(0).Values(4))
                Me.MOPPORTUNITYGENDER.Caption = "Gender"
                Me.MOPPORTUNITYBIRTHDATE.Visible = True
            Else
                Me.MOPPORTUNITYGENDER.Caption = "Category"
                Me.MOPPORTUNITYGENDER.Value = reply.Rows(0).Values(7)
                Me.MOPPORTUNITYBIRTHDATE.Visible = False
            End If

            Dim offerSoleSponsorship As String
            offerSoleSponsorship = reply.Rows(0).Values(2)

            If offerSoleSponsorship IsNot Nothing Then
                If offerSoleSponsorship.Equals(1) Then
                    Me.ISSOLESPONSORSHIP.Visible = True
                ElseIf offerSoleSponsorship.Equals(0) Then
                    UpdateChkSoleSponsorship(False)
                    Me.ISSOLESPONSORSHIP.Visible = False
                End If
            End If

            ' commented by Memphis so the greatest need search isn't ever disabled:
            'Me.SPONSORSHIPLOCATIONID.Enabled = False
            'Me.SPROPPAGERANGEID.Enabled = False
            'Me.GENDERCODE.Enabled = False
            'Me.ISORPHANEDCODE.Enabled = False
            'Me.ISHIVPOSITIVECODE.Enabled = False
            'Me.HASCONDITIONCODE.Enabled = False
            'Me.SPROPPPROJECTCATEGORYCODEID.Enabled = False

            ' try to put the found child in the children list:
            ' ADDCHILDTOLIST
            Me.FINDOPPORTUNITY.Caption = "Add Opportunity"

            Me.MOPPORTUNITYIMAGE.Visible = True
            Me.MOPPORTUNITYIMAGE.ValueDisplayStyle = ValueDisplayStyle.GoodImageOnly

            HandleSoleSponsorship(False)
        Else
            MatchedOpportunityVisible(False)
            Me.MOPPORTUNITYIMAGE.Visible = True
            Me.MOPPORTUNITYIMAGE.ValueDisplayStyle = ValueDisplayStyle.BadImageAndText
            Me.MOPPORTUNITYIMAGE.Value = "No matching opportunity was found."
        End If
    End Sub
    Private Sub MatchedOpportunityVisible(ByVal visible As Boolean)
        'matched opportunity panel
        With Me
            .MATCHEDOPPORTUNITY.Visible = visible
            .MOPPORTUNITYNAME.Visible = visible
            .MOPPORTUNITYLOCATION.Visible = visible
            .MOPPORTUNITYLOOKUPID.Visible = visible
            .MOPPORTUNITYGENDER.Visible = visible
            .MOPPORTUNITYBIRTHDATE.Visible = visible
        End With
    End Sub


    Private Sub OpportunitySelectionEnableDisable(ByVal enabled As Boolean)
        ' commented by Memphis so the greatest need search isn't ever disabled:
        'With Me
        '    .SELECTOPPORTUNITYID.Enabled = enabled
        Me.SPONSORSHIPOPPORTUNITYIDCHILD.Enabled = True
        '    .SPONSORSHIPOPPORTUNITYIDPROJECT.Enabled = enabled
        '    .RESERVATIONKEY.Enabled = enabled
        '    .RESERVEDOPPORTUNITYIDCHILD.Enabled = enabled
        '    .SPONSORSHIPLOCATIONID.Enabled = enabled
        '    If Not enabled Then
        '        .GENDERCODE.Enabled = enabled
        '        .SPROPPAGERANGEID.Enabled = enabled
        '        .ISORPHANEDCODE.Enabled = enabled
        '        .ISHIVPOSITIVECODE.Enabled = enabled
        '        .HASCONDITIONCODE.Enabled = enabled
        '        .SPROPPPROJECTCATEGORYCODEID.Enabled = enabled
        '    End If
        '    .FINDOPPORTUNITY.Enabled = enabled
        'End With
    End Sub


    Private Sub GreatestNeedFieldsEnableDisable(ByVal enabled As Boolean)
        ' commented by Memphis so the greatest need search isn't ever disabled:
        'With Me
        '    .SPONSORSHIPLOCATIONID.Enabled = enabled
        '    If Not enabled Then
        '        .GENDERCODE.Enabled = enabled
        '        .SPROPPAGERANGEID.Enabled = enabled
        '        .ISORPHANEDCODE.Enabled = enabled
        '        .ISHIVPOSITIVECODE.Enabled = enabled
        '        .HASCONDITIONCODE.Enabled = enabled
        '        .SPROPPPROJECTCATEGORYCODEID.Enabled = enabled
        '    End If
        '    .FINDOPPORTUNITY.Enabled = enabled
        'End With
    End Sub

    Private Sub UpdateChkSoleSponsorship(ByVal checked As Boolean)
        _processingSoleSponsorship = True
        Me.ISSOLESPONSORSHIP.Value = checked
        _processingSoleSponsorship = False
    End Sub

    Private Sub HandleSoleSponsorship(ByVal overwriteCheckbox As Boolean)
        Dim oppyId As String
        Dim sponsorCount As Integer = 0
        oppyId = GetOpportunity()

        If _soleSponsorshipOverrides Then
            Dim locationId As String = Nothing

            If Not String.IsNullOrEmpty(oppyId) AndAlso Not oppyId.Equals("00000000-0000-0000-0000-000000000000") Then
                'get location and sponsor count for opportunity
                Dim reply As DataFormLoadReply
                reply = SoleSponsorshipInfo(oppyId)

                Dim locationIdG As Guid
                reply.DataFormItem.Values.TryGetValue("SPONSORSHIPLOCATIONID", locationIdG)
                locationId = locationIdG.ToString
                reply.DataFormItem.Values.TryGetValue("SPONSORCOUNT", sponsorCount)

            ElseIf SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed AndAlso Not Me.SPONSORSHIPLOCATIONID.Value.Equals(Guid.Empty) Then
                locationId = Me.SPONSORSHIPLOCATIONID.Value.ToString
            End If

            If locationId IsNot Nothing Then
                'call sole sponsorship
                Dim offerSoleSponsorship As Boolean

                Dim request As New DataListLoadRequest
                request.DataListID = New Guid("dfd49aaf-1dfc-453f-9f8c-325651df76e1")
                request.SecurityContext = _securityContext
                request.Parameters = New DataFormItem
                request.Parameters.Values.Add("SPONSORSHIPOPPORTUNITYGROUPID", _groupId)
                request.Parameters.Values.Add("SPONSORSHIPLOCATIONID", locationId)

                Dim reply As DataListLoadReply
                reply = DataListLoad(request, Me.GetRequestContext)

                offerSoleSponsorship = CBool(reply.Rows(0).Values(0))

                If offerSoleSponsorship Then
                    Me.ISSOLESPONSORSHIP.Visible = True
                    If sponsorCount = 0 Then
                        Me.ISSOLESPONSORSHIP.Enabled = True
                        If overwriteCheckbox Then
                            UpdateChkSoleSponsorship(_cacheSoleSponsorship)
                        End If
                    Else
                        'Opportunity already has other sponsors.
                        Me.ISSOLESPONSORSHIP.Enabled = False
                        UpdateChkSoleSponsorship(False)
                    End If
                Else
                    'Sole sponsorship not an option in this location.
                    Me.ISSOLESPONSORSHIP.Visible = False
                    UpdateChkSoleSponsorship(False)
                End If
            Else
                'Location not determined, can't offer sole sponsorship.
                Me.ISSOLESPONSORSHIP.Visible = False
                UpdateChkSoleSponsorship(False)
            End If
        ElseIf Me.ISSOLESPONSORSHIP.Visible Then
            If Not String.IsNullOrEmpty(oppyId) AndAlso Not oppyId.Equals("00000000-0000-0000-0000-000000000000") Then
                'get location and sponsor count for opportunity
                Dim reply As DataFormLoadReply
                reply = SoleSponsorshipInfo(oppyId)

                reply.DataFormItem.Values.TryGetValue("SPONSORCOUNT", sponsorCount)

                If sponsorCount = 0 Then
                    Me.ISSOLESPONSORSHIP.Enabled = True
                    If overwriteCheckbox Then
                        UpdateChkSoleSponsorship(_cacheSoleSponsorship)
                    End If
                Else
                    'Opportunity already has other sponsors.
                    Me.ISSOLESPONSORSHIP.Enabled = False
                    UpdateChkSoleSponsorship(False)
                End If
            Else
                'No opportunity selected, sole sponsorship allowed.
                Me.ISSOLESPONSORSHIP.Enabled = True
                If overwriteCheckbox Then
                    UpdateChkSoleSponsorship(_cacheSoleSponsorship)
                End If
            End If
        End If
    End Sub

    Private Function SoleSponsorshipInfo(ByVal opportunityId As String) As DataFormLoadReply
        Dim request As New DataFormLoadRequest
        request.FormID = New Guid("b5b49bc6-70ca-416d-aa1b-7b56187ad833")
        request.RecordID = opportunityId
        request.SecurityContext = _securityContext

        Return DataFormLoad(request, Me.GetRequestContext)
    End Function

    Private Sub _issolesponsorship_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _issolesponsorship.ValueChanged
        If Not _processingSoleSponsorship Then
            _cacheSoleSponsorship = Me.ISSOLESPONSORSHIP.Value
        End If
    End Sub

    Private Sub HandleSponsorshipProgram(ByVal overwriteValues As Boolean, ByVal programChange As Boolean)
        '_groupLockedFields.Clear()

        If Not Me.SPONSORSHIPPROGRAMID.HasValue Then
            Exit Sub
        End If


        Dim programInfoReply As DataListResultRow = _programHelper.GetProgramInfo

        _groupId = programInfoReply.Values(0)

        Dim groupInfoReply As DataListResultRow = _programHelper.GetGroupInfo(_groupId)

        Dim _genderCode, _isHivPositiveCode, _orphanedCode, _hasConditionCode As Integer
        'Dim agerange As String

        If _previousGroupId IsNot Nothing And _previousGroupId <> _groupId Then
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.SearchDisplayText = ""
        End If

        _soleSponsorshipOverrides = CBool(groupInfoReply.Values(14))
        If Me.ISSOLESPONSORSHIP.Value AndAlso Not overwriteValues Then
            Me.ISSOLESPONSORSHIP.Visible = True
        Else
            Me.ISSOLESPONSORSHIP.Visible = CBool(groupInfoReply.Values(3))
            If _soleSponsorshipOverrides Then
                HandleSoleSponsorship(True)
            ElseIf Not Me.ISSOLESPONSORSHIP.Visible Then
                UpdateChkSoleSponsorship(False)
            Else
                'If this was originally a sole sponsorship, retain that when allowed.
                UpdateChkSoleSponsorship(IIf(Me.ISSOLESPONSORSHIP.Value, Me.ISSOLESPONSORSHIP.Value, _cacheSoleSponsorship))
            End If
        End If

        Integer.TryParse(groupInfoReply.Values(2), _opportunitytype)
        Integer.TryParse(groupInfoReply.Values(7), _genderCode)
        Integer.TryParse(groupInfoReply.Values(8), _isHivPositiveCode)
        Integer.TryParse(groupInfoReply.Values(9), _orphanedCode)
        Integer.TryParse(groupInfoReply.Values(10), _hasConditionCode)

        If Not _amountSet OrElse programChange Then
            Dim baseCurrencyId As Guid
            baseCurrencyId = New Guid(programInfoReply.Values(3))

            'For now, only default amount from program if the program's base currency agrees w/ both the user's base
            'currency and the transaction currency.
            'FUTURE:  Compare user's base currency to program's base currency, and if same, update the spon
            'base amount/currency and reverse convert to the transaction currency if possible (otherwise error and revert amounts/currencies).

            If baseCurrencyId.Equals(Me.BASECURRENCYID.Value) AndAlso _
               baseCurrencyId.Equals(Me.TRANSACTIONCURRENCYID.Value) Then
                Dim amount As String
                amount = programInfoReply.Values(1).Substring(0, programInfoReply.Values(1).IndexOf(".", StringComparison.OrdinalIgnoreCase) + 3)
                Me.AMOUNT.Value = amount
                UpdateBaseAmount()
                'LoadTextBoxFromValue(txt_AMOUNT, Xml.XmlConvert.ToDecimal(amount), GetFormField(txt_AMOUNT))
                'UpdateTransactionAmount()
                ' Me.BASEAMOUNT.Value = amount
            End If
            _amountSet = True
        End If

        If Not overwriteValues AndAlso Not Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value.Equals(Guid.Empty) _
                                        OrElse Not Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Value.Equals(Guid.Empty) Then
            If Me.SPONSORSHIPPROGRAMID.Enabled Then
                OpportunitySelectionEnableDisable(True)
            Else
                'Reassigning
                OpportunitySelectionEnableDisable(False)
                'prevent overwriting of sole sponsorship checkbox
                Me.ISSOLESPONSORSHIP.Enabled = False
                _soleSponsorshipOverrides = False
                SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity

                'memphis commented out to test lookup
                'Me.SPONSORSHIPOPPORTUNITYIDCHILD.Enabled = False


                Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Enabled = False
                Me.SPONSORSHIPPROGRAMID.Enabled = False
            End If
        Else
            OpportunitySelectionEnableDisable(True)
            If Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value.Equals(Guid.Empty) _
                        AndAlso Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Value.Equals(Guid.Empty) _
                        AndAlso Me.RESERVATIONKEY.Value.Equals(Guid.Empty) Then
                SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed
            End If
        End If

        SetFields()

        _programHelper.SetGroup(groupInfoReply, MatchValid())

        _groupLockedFields = _programHelper.groupLockedFields()

        If programInfoReply.Values(2).Equals("0") Then
            'Not an affiliate program.
            _isAffiliate = False
            UpdatePaymentFieldsEnabled()
            EnableFieldsForConstituent()
            If Me.FINDERNUMBER.Value.Equals(0) OrElse Len(Me.FINDERNUMBER.Value.ToString) = 0 OrElse Not _finderNumberHelper.FinderNumberIsValid() Then
                Me.APPEALID.Enabled = True
                Me.MAILINGID.Enabled = True
            End If
            Me.GIFTRECIPIENT.Enabled = Not Transferring()
        Else
            _isAffiliate = True
            UpdatePaymentFieldsEnabled()
            EnableFieldsForConstituent()
            Me.GIFTRECIPIENT.Value = False
            Me.GIFTRECIPIENT.Enabled = False
            Me.AUTOPAY.Value = False
            Me.CONSTITUENTACCOUNTID.Visible = False
            Me.REFERENCEDATE.Visible = False
        End If

        'clear out reserved child
        If Not Me.RESERVEDOPPORTUNITYIDCHILD.Value.Equals(Guid.Empty) And _previousGroupId <> _groupId Then
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Me.RESERVEDOPPORTUNITYIDCHILD.Value = Nothing
            Me.RESERVATIONKEY.Value = Nothing
        End If
        'set _previousGroupid
        _previousGroupId = _groupId

    End Sub


    Private Sub SetFields()
        Dim child As Boolean = (_opportunitytype = CHILD_OPPORTUNITY)

        If Not child AndAlso SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseAReservedOpportunity Then
            SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed
        End If

        'Specific opportunity
        Me.SPONSORSHIPOPPORTUNITYIDCHILD.Visible = child
        If Not SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity OrElse Not child Then
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.SearchDisplayText = Nothing
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.Required = False
        Else
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.Required = True
        End If

        'memphis commented out to test lookup
        'Me.SPONSORSHIPOPPORTUNITYIDCHILD.Enabled = SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity

        'added to enable/disable the Children List and Remove button:
        'ChildrenListEnableDisable(SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity)
        'Me.CHILDREN.Enabled = SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity
        'Me.REMOVESELECTEDCHILD.Enabled = SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity

        Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Visible = Not child
        If Not SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity OrElse child Then
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Me.SPONSORSHIPOPPORTUNITYIDPROJECT.SearchDisplayText = Nothing
            Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Required = False
        Else
            Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Required = True
        End If
        Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Enabled = (SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity)

        'Reserved opportunity
        Me.RESERVATIONKEY.Enabled = (SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseAReservedOpportunity)
        If Not SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseAReservedOpportunity Then
            Me.RESERVATIONKEY.Value = Nothing
            Me.RESERVATIONKEY.SearchDisplayText = Nothing
            Me.RESERVATIONKEY.Required = False
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Me.RESERVEDOPPORTUNITYIDCHILD.Value = Nothing
            Me.RESERVEDOPPORTUNITYIDCHILD.SearchDisplayText = Nothing
            Me.RESERVEDOPPORTUNITYIDCHILD.Required = False
        Else
            Me.RESERVATIONKEY.Required = True
        End If
        If Me.RESERVATIONKEY.Value.Equals(Guid.Empty) Then
            Me.RESERVEDOPPORTUNITYIDCHILD.Enabled = False
        End If

        'Criteria       
        If Not SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed Then
            HandleCancel()
            FINDOPPORTUNITY.Caption = BUTTONLABEL_MATCH
            MatchedOpportunityVisible(False)
            MATCHEDOPPORTUNITYID.Value = Nothing
            'display the greatest need search no matter what:
            'GreatestNeedFieldsEnableDisable(False)
            Me.MOPPORTUNITYIMAGE.Visible = False
        ElseIf Me.FINDOPPORTUNITY.Caption = BUTTONLABEL_MATCH Then
            Me.MOPPORTUNITYIMAGE.Visible = True
            Me.MOPPORTUNITYIMAGE.ValueDisplayStyle = ValueDisplayStyle.WarningImageAndText
            Me.MOPPORTUNITYIMAGE.Value = "No opportunity currently selected."
            GreatestNeedFieldsEnableDisable(True)

            If child Then
                EnableCriteriaField(Me.GENDERCODE)
                EnableCriteriaField(Me.ISORPHANEDCODE)
                EnableCriteriaField(Me.ISHIVPOSITIVECODE)
                EnableCriteriaField(Me.HASCONDITIONCODE)
                EnableCriteriaField(Me.SPROPPAGERANGEID)
                Me.SPROPPPROJECTCATEGORYCODEID.Enabled = False
                'Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Visible = False
            Else
                Me.GENDERCODE.Enabled = False
                Me.SPROPPAGERANGEID.Enabled = False
                Me.ISORPHANEDCODE.Enabled = False
                Me.ISHIVPOSITIVECODE.Enabled = False
                Me.HASCONDITIONCODE.Enabled = False
                EnableCriteriaField(Me.SPROPPPROJECTCATEGORYCODEID)
            End If
        End If
        HandleSoleSponsorship(False)
    End Sub

    Private Sub HandleCancel()
        If Me.MATCHEDOPPORTUNITYID.Value.Equals(Guid.Empty) Then
            lockHelper.UnlockOpportunity(Me.GetRequestContext)
        Else
            lockHelper.UnlockOpportunity(Me.MATCHEDOPPORTUNITYID.Value.ToString, Me.GetRequestContext)
        End If
    End Sub

    Private Function Transferring() As Boolean
        Return Me.DataFormInstanceId.Equals(New Guid(TRANSFERFORMID)) OrElse _
           Me.DataFormInstanceId.Equals(New Guid(OVERRIDETRANSFERFORMID))
    End Function

    Private Sub _findopportunity_InvokeAction(ByVal sender As Object, ByVal e As UIModeling.Core.InvokeActionEventArgs) Handles _findopportunity.InvokeAction

        If Me.FINDOPPORTUNITY.Caption.Equals(BUTTONLABEL_MATCH) Then
            MatchOpportunity()
        Else
            'add the matched child to the list of children,
            'and clear the matched child so user can match
            'another
            'lockHelper.LockOpportunity(Me.MATCHEDOPPORTUNITYID.Value.ToString, _soleSponsorshipOverrides, Me.GetRequestContext)
            If LockThisChild(Me.MATCHEDOPPORTUNITYID.Value) Then
                'set the lookupid
                _lookupid = Me.MOPPORTUNITYLOOKUPID.Value.ToString()
                AddChildToList()
            Else
                DisplayPrompt("Unable to lock this selected child!")
            End If

            ClearMatchedOpportunity()
        End If
    End Sub

    Private Sub _removeselectedchild_InvokeAction(ByVal sender As Object, ByVal e As UIModeling.Core.InvokeActionEventArgs) Handles _removeselectedchild.InvokeAction
        'delete the selected child
        Dim childId As Guid = New Guid(Me.CHILDREN.Selection.ActiveRecord.NAME.Value.ToString())
        Me.CHILDREN.Selection.Delete()
        If Not childId = Guid.Empty Then
            DeleteChildSelectionLock(childId)
        End If
    End Sub

    Private Sub ClearMatchedOpportunity()
        HandleCancel()
        Dim child As Boolean = (_opportunitytype = CHILD_OPPORTUNITY)
        Me.SPONSORSHIPLOCATIONID.Enabled = True
        If child Then
            EnableCriteriaField(Me.GENDERCODE)
            EnableCriteriaField(Me.ISORPHANEDCODE)
            EnableCriteriaField(Me.ISHIVPOSITIVECODE)
            EnableCriteriaField(Me.HASCONDITIONCODE)
            EnableCriteriaField(Me.SPROPPAGERANGEID)
        Else
            EnableCriteriaField(Me.SPROPPPROJECTCATEGORYCODEID)
        End If
        Me.FINDOPPORTUNITY.Caption = "Find" 'BUTTONLABEL_MATCH"
        Me.MATCHEDOPPORTUNITYID.Value = Nothing
        Me.MOPPORTUNITYIMAGE.Visible = True
        Me.MOPPORTUNITYIMAGE.ValueDisplayStyle = ValueDisplayStyle.WarningImageAndText
        Me.MOPPORTUNITYIMAGE.Value = "No opportunity currently selected."
        MatchedOpportunityVisible(False)
        HandleSoleSponsorship(False)
    End Sub

    Private Sub EnableCriteriaField(ByVal criteriaField As UIField)
        If Not _groupLockedFields.Contains(criteriaField) Then
            criteriaField.Enabled = True
        End If
    End Sub


    Protected Overrides Sub OnValidating(ByVal e As Global.Blackbaud.AppFx.UIModeling.Core.ValidatingEventArgs)
        MyBase.OnValidating(e)

        UpdatePaymentFieldsValues()

        'If Me.REVENUESCHEDULESTARTDATE.Value < Me.STARTDATE.Value Then
        '    Throw New Global.Blackbaud.AppFx.UIModeling.Core.InvalidUIModelException(String.Format(CultureInfo.CurrentCulture, My.Resources.Errors.RecurringStartDateNotBeforeDate), Me.REVENUESCHEDULESTARTDATE)
        'End If

        ValidateInstallments()
    End Sub

    Private Sub SponsorshipAddFormUIModel_SaveFailed(ByVal sender As Object, ByVal e As UIModeling.Core.SaveFailedEventArgs) Handles Me.SaveFailed
        If _tempBatchNumber IsNot Nothing Then
            Me.Fields.Add(_tempBatchNumber)
            _tempBatchNumber = Nothing
        End If
    End Sub

    Private Sub SponsorshipAddFormUIModel_Saving(ByVal sender As Object, ByVal e As UIModeling.Core.SavingEventArgs) Handles Me.Saving
        If _currentForm = CURRENTFORM.REASSIGN Then
            _tempBatchNumber = _batchnumber
            Me.Fields.Remove(_batchnumber)
        End If
    End Sub

    Private Sub SponsorshipAddFormUIModel_Canceling(ByVal sender As Object, ByVal e As UIModeling.Core.CancelingEventArgs) Handles Me.Canceling
        Try
            'need to unlock any children in the children selected list collection field
            'CALL UNLOCK CHILD HERE
            UnlockSelectedChildren()
            lockHelper.UnlockOpportunity(GetOpportunity(), Me.GetRequestContext)
        Catch ex As ServiceException
            Throw New ServiceException("unlocked")
        End Try
    End Sub

    Private Sub SponsorshipAddFormUIModel_Validating(ByVal sender As Object, ByVal e As UIModeling.Core.ValidatingEventArgs) Handles Me.Validating
        If _requireLocationId And Me.SPONSORSHIPLOCATIONID.Value.Equals(Guid.Empty) Then
            Me.SPONSORSHIPLOCATIONID.Enabled = True
            e.Valid = False
            e.InvalidReason = "Please enter a location."
        End If
    End Sub

    Private Sub _sponsorshiplocationid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _sponsorshiplocationid.ValueChanged
        HandleSoleSponsorship(True)
        'If Me.SPONSORSHIPLOCATIONID.SearchFieldOverrides IsNot Nothing And Not _lockOverrides Then
        '    Me.SPONSORSHIPLOCATIONID.SearchFieldOverrides(0).DefaultValueText = Me.SPONSORSHIPLOCATIONID.Value.ToString
        '    Me.SPONSORSHIPLOCATIONID.SearchFieldOverrides(1).DefaultValueText = Me.SPONSORSHIPLOCATIONID.SearchDisplayText
        'End If
    End Sub

    Private Sub _selectopportunityid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _selectopportunityid.ValueChanged
        If _initialLoadComplete Then
            SetFields()
        End If
    End Sub
    Private Sub PromptCallBack(ByVal sender As Object, ByVal e As UIPromptResponseEventArgs)

        If DirectCast(e.Response, Boolean) Then
            If Me.SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.FindAMatchingOpportunityOfGreatestNeed Then
                ClearMatchedOpportunity()
            ElseIf Me.SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseASpecificOpportunity Then
                lockHelper.UnlockOpportunity(Me.GetRequestContext)
                Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value = Nothing
                Me.SPONSORSHIPOPPORTUNITYIDPROJECT.Value = Nothing
            ElseIf Me.SELECTOPPORTUNITYID.Value = SponsorshipAddFormUIModel.SELECTOPPORTUNITYIDS.ChooseAReservedOpportunity Then
                lockHelper.UnlockOpportunity(Me.GetRequestContext)
                Me.RESERVEDOPPORTUNITYIDCHILD.Value = Nothing
            End If
            If _changedOpportunity = CHANGEOPPORTUNITY.FINANCIAL_SPONSOR Then
                HandleChangeFinancialSponsor()
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.CORRESPONDING_SPONSOR Then
                HandleChangeCorrespondingSponsor()
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.PROGRAM Then
                HandleChangeProgramOpportunity()
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.GIFTRECIPIENT Then
                HandleChangeGiftRecipient()
            End If
        Else
            If _changedOpportunity = CHANGEOPPORTUNITY.FINANCIAL_SPONSOR Then
                Me.REVENUECONSTITUENTID.Value = New Guid(_revenueConstituent)
                Me.REVENUECONSTITUENTID.SearchDisplayText = _revenueConstituentName
                Me.SPONSORSHIPCONSTITUENTID.Value = New Guid(_sponsorshipConstituent)
                Me.SPONSORSHIPCONSTITUENTID.SearchDisplayText = _sponsorshipConstituentName
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.CORRESPONDING_SPONSOR Then
                Me.SPONSORSHIPCONSTITUENTID.Value = New Guid(_sponsorshipConstituent)
                Me.SPONSORSHIPCONSTITUENTID.SearchDisplayText = _sponsorshipConstituentName
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.PROGRAM Then
                Me.SPONSORSHIPPROGRAMID.Value = _programValue
            ElseIf _changedOpportunity = CHANGEOPPORTUNITY.GIFTRECIPIENT Then
                Me.GIFTRECIPIENT.Value = True
                Me.SPONSORSHIPCONSTITUENTID.Value = New Guid(_sponsorshipConstituent)
                Me.SPONSORSHIPCONSTITUENTID.SearchDisplayText = _sponsorshipConstituentName
            End If
        End If
        _changedOpportunity = Nothing
    End Sub

#Region "Payment Tab"

    Private Sub UpdatePaymentFieldsEnabled()
        'credit card and direct debit fields are only enabled if auto-paying

        Dim b = Me.AUTOPAY.Value AndAlso (Me.REVENUECONSTITUENTID.Value <> Guid.Empty OrElse _currentForm = CURRENTFORM.REASSIGN) AndAlso Not _isAffiliate

        If _currentForm = CURRENTFORM.TRANSFER Then
            b = False
        End If

        Me.PAYMENTMETHODCODE.Enabled = b

        Me.CREDITTYPECODEID.Enabled = b
        Me.CREDITCARDNUMBER.Enabled = b
        Me.CARDHOLDERNAME.Enabled = b
        Me.EXPIRESON.Enabled = b

        Me.REFERENCEDATE.Enabled = b
        Me.REFERENCENUMBER.Enabled = b
        Me.CONSTITUENTACCOUNTID.Enabled = b

    End Sub

    Private Sub UpdatePaymentFieldsRequired()
        Dim b = Me.AUTOPAY.Value

        'Account is required if auto-paying by direct debit
        Me.CONSTITUENTACCOUNTID.Required = (b AndAlso _
            (Me.PAYMENTMETHODCODE.Value = PledgeAddFormUIModel.PAYMENTMETHODCODES.DirectDebit))
    End Sub

    Private Sub UpdatePaymentFieldsVisible()
        Me.CREDITTYPECODEID.Visible = False
        Me.CREDITCARDNUMBER.Visible = False
        Me.CARDHOLDERNAME.Visible = False
        Me.EXPIRESON.Visible = False
        Me.REFERENCEDATE.Visible = False
        Me.REFERENCENUMBER.Visible = False
        Me.CONSTITUENTACCOUNTID.Visible = False

        Select Case Me.PAYMENTMETHODCODE.Value
            Case PAYMENTMETHODCODES.CreditCard
                Me.CREDITTYPECODEID.Visible = True
                Me.CREDITCARDNUMBER.Visible = True
                Me.CARDHOLDERNAME.Visible = True
                Me.EXPIRESON.Visible = True

            Case PAYMENTMETHODCODES.DirectDebit
                Me.REFERENCEDATE.Visible = True
                Me.REFERENCENUMBER.Visible = True
                Me.CONSTITUENTACCOUNTID.Visible = True
        End Select
    End Sub

    Private Sub UpdatePaymentFieldsValues()
        ' Clear values on hidden fields.

        Select Case Me.PAYMENTMETHODCODE.Value
            Case PAYMENTMETHODCODES.CreditCard
                Me.REFERENCEDATE.Value = Nothing
                Me.REFERENCENUMBER.Value = Nothing
                Me.CONSTITUENTACCOUNTID.Value = Nothing

            Case PAYMENTMETHODCODES.DirectDebit
                Me.CREDITTYPECODEID.Value = Nothing
                Me.CREDITCARDNUMBER.Value = Nothing
                Me.CARDHOLDERNAME.Value = Nothing
                Me.EXPIRESON.Value = Nothing

        End Select
    End Sub

    Private Sub PopulateMarketingFieldsFromSourceCode(ByVal code As String)

        Dim request As New DataFormLoadRequest()
        request.FormID = New Guid("A2D70B98-EE76-4812-9DC7-36D4F8E18C65") ' SourceCodeMapSearch.View.xml
        request.RecordID = code
        request.SecurityContext = Me.GetRequestSecurityContext()

        Dim reply As DataFormLoadReply = Nothing

        Try
            Dim svc = New AppFxWebService(Me.GetRequestContext())
            reply = svc.DataFormLoad(request)
        Catch eat As Exception
            ' yum!
        End Try

        If Not reply Is Nothing Then
            _sourcecode.Value = code

            reply.DataFormItem.TryGetValue("APPEALID", Me.APPEALID.Value)
            Me.APPEALID.UpdateDisplayText()

            reply.DataFormItem.TryGetValue("SEGMENTATIONID", Me.MAILINGID.Value)
            Me.MAILINGID.UpdateDisplayText()
        End If

    End Sub

    Private Sub EnableFieldsForConstituent()
        Dim hasConstituent = (_revenueconstituentid.Value <> Guid.Empty OrElse _currentForm = CURRENTFORM.REASSIGN) AndAlso Not _isAffiliate

        With Me
            .AMOUNT.Enabled = hasConstituent
            If _currentForm = CURRENTFORM.TRANSFER Then
                .AUTOPAY.Enabled = False
            Else
                .AUTOPAY.Enabled = hasConstituent
            End If
            .FREQUENCYCODE.Enabled = hasConstituent
            .REVENUESCHEDULESTARTDATE.Enabled = hasConstituent
            .SOURCECODE.Enabled = hasConstituent
            .SOURCECODELOOKUP.Enabled = hasConstituent
            .APPEALID.Enabled = hasConstituent
            .MAILINGID.Enabled = hasConstituent
            .CHANNELCODEID.Enabled = hasConstituent
            .REFERENCE.Enabled = hasConstituent
            .CATEGORYCODEID.Enabled = hasConstituent
            .INSTALLMENTS.Enabled = hasConstituent
            .SENDREMINDER.Enabled = hasConstituent
            .CURRENCYACTION.Enabled = hasConstituent
            .BASEAMOUNT.Enabled = hasConstituent
            .DONOTACKNOWLEDGE.Enabled = hasConstituent
        End With

        UpdatePaymentFieldsEnabled()
    End Sub

    Private Sub ValidateInstallments()
        Dim lastDate As DateTime = Me.REVENUESCHEDULESTARTDATE.Value

        For Each installment In Me.INSTALLMENTS.Value
            'make sure that the installments are in date order
            If installment.DATE.Value < lastDate Then
                Throw New Global.Blackbaud.AppFx.UIModeling.Core.InvalidUIModelException("Installment dates are out of sequence.", Me.INSTALLMENTS)
            Else
                lastDate = installment.DATE.Value
            End If
        Next
    End Sub

    Private Shared Function CreateInstallment(ByVal scheduleDate As Date, ByVal amount As Decimal) As SponsorshipAddFormWrappedINSTALLMENTSUIModel
        Dim o As New SponsorshipAddFormWrappedINSTALLMENTSUIModel
        o.AMOUNT.Value = amount
        o.DATE.Value = scheduleDate

        Return o
    End Function

    Private Sub LoadInstallments()
        If Me.REVENUESCHEDULESTARTDATE.Value = Date.MinValue Then
            Return
        End If

        Dim results = ScheduleGenerator.GetFixedAmountSchedule(CType(_frequencycode.Value, ScheduleGenerator.Frequency), _revenueschedulestartdate.Value, _revenuescheduleenddate.Value, _amount.Value)

        Dim q = From item In results Select CreateInstallment(item.ScheduleDate, item.Amount)

        _installments.Value.Clear()

        _installments.Value.AddRange(q)

        For Each installment In Me.INSTALLMENTS.Value
            installment.TRANSACTIONCURRENCYID.Value = Me.TRANSACTIONCURRENCYID.Value
        Next
    End Sub

    Private Sub UpdateBaseAmount()
        Me.BASEAMOUNT.Value = CurrencyMath.ConvertCurrency(Me.AMOUNT.Value, Me.EXCHANGERATE.Value, Me.BASECURRENCYDECIMALDIGITS.Value, Me.BASECURRENCYROUNDINGTYPECODE.Value)
    End Sub

    Private Sub SetBaseExchangeRate()
        Dim request As New DataListLoadRequest()
        request.DataListID = New Guid("EC9C9CEB-32FC-48CE-B3D3-EE200048F327")
        request.MaxRows = 1
        request.SecurityContext = Me.GetRequestSecurityContext()
        request.Parameters = New DataFormItem()
        request.Parameters.SetValue("FROMCURRENCYID", Me.TRANSACTIONCURRENCYID.Value)
        request.Parameters.SetValue("TOCURRENCYID", Me.BASECURRENCYID.Value)
        request.Parameters.SetValue("ENDDATE", Me.STARTDATE.Value)
        request.Parameters.SetValue("TYPECODE", 1)  'Only look for daily rates.

        Dim svc = New AppFxWebService(Me.GetRequestContext())
        Dim reply = svc.DataListLoad(request)
        If reply IsNot Nothing And reply.Rows.Count > 0 Then
            Me.EXCHANGERATE.Value = Xml.XmlConvert.ToDecimal(reply.Rows(0).Values(1))
            Me.BASEEXCHANGERATEID.Value = New Guid(reply.Rows(0).Values(0))
        Else
            'We were unable to find a rate that matched the given criteria, so clear the value.
            Me.EXCHANGERATE.Value = 0D
            Me.BASEEXCHANGERATEID.Value = Nothing
        End If
    End Sub

    Private Sub _appealid_Search(ByVal sender As Object, ByVal e As UIModeling.Core.SearchEventArgs) Handles _appealid.Search
        Dim searchModel = TryCast(e.SearchModel, RevenueAppealSearchUIModel)
        If searchModel IsNot Nothing AndAlso _revenueconstituentid.Value <> Guid.Empty Then
            searchModel.CONSTITUENTID.Value = _revenueconstituentid.Value
        End If
    End Sub

    Private Sub _autopay_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _autopay.ValueChanged
        UpdatePaymentFieldsEnabled()
        UpdatePaymentFieldsRequired()
    End Sub

    Private Sub _frequencycode_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _frequencycode.ValueChanged
        If Not Loading Then
            LoadInstallments()
        End If
    End Sub

    Private Sub _paymentmethodcode_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _paymentmethodcode.ValueChanged
        If Not Loading Then
            UpdatePaymentFieldsVisible()
            UpdatePaymentFieldsRequired()
        End If
    End Sub

    Private Sub _startdate_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _startdate.ValueChanged
        If Me.TRANSACTIONCURRENCYID.Value <> Guid.Empty AndAlso Me.BASECURRENCYID.Value <> Guid.Empty AndAlso Me.STARTDATE.HasValue() _
            AndAlso Me.BASEEXCHANGERATEID.Value <> Guid.Empty AndAlso Me.BASEEXCHANGERATEID.Value <> _spotRateID Then

            SetBaseExchangeRate()
        End If
    End Sub

    Private Sub _revenueschedulestartdate_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _revenueschedulestartdate.ValueChanged
        If Not Loading Then
            LoadInstallments()
        End If
    End Sub

    Private Sub _revenuescheduleenddate_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _revenuescheduleenddate.ValueChanged
        If Not Loading Then
            LoadInstallments()
        End If
    End Sub

    Private Sub _sourcecode_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _sourcecode.ValueChanged

        If Not String.IsNullOrEmpty(_sourcecode.Value) Then
            Dim request As New SearchListLoadRequest()
            request.SearchListID = New Guid("EF764BB7-FDF0-45C2-A50F-294BF460F200") ' SourceCodeMap.Search.xml
            request.SecurityContext = Me.GetRequestSecurityContext()
            request.Filter = New DataFormItem
            request.Filter.SetValue("SOURCECODE", _sourcecode.Value)
            request.Filter.SetValue("EXACTMATCHONLY", True)
            request.Filter.SetValue("INCLUDEMARKETINGEFFORTCODES", True)
            request.Filter.SetValue("INCLUDEWHITEMAILCODES", True)
            request.Filter.SetValue("INCLUDEINACTIVEWHITEMAILCODES", True)
            request.MaxRecords = 2

            Dim reply As SearchListLoadReply = SearchListLoad(request, Me.GetRequestContext())

            If Not reply Is Nothing AndAlso Not reply.Output Is Nothing AndAlso reply.Output.RowCount = 1 Then
                PopulateMarketingFieldsFromSourceCode(reply.Output.Rows(0).Values(1))
            End If
        End If

    End Sub

    Private Sub _sourcecodelookup_Search(ByVal sender As Object, ByVal e As UIModeling.Core.SearchEventArgs) Handles _sourcecodelookup.Search

        With DirectCast(e.SearchModel, SourceCodeMapSearchUIModel)
            .SOURCECODE.Value = _sourcecode.Value
            .CONSTITUENTID.Value = _revenueconstituentid.Value
            .CONSTITUENTID.UpdateDisplayText()
            .NAME.Value = _mailingid.SearchDisplayText
        End With

    End Sub

    Private Sub _sourcecodelookup_SearchItemSelected(ByVal sender As Object, ByVal e As UIModeling.Core.SearchItemSelectedEventArgs) Handles _sourcecodelookup.SearchItemSelected

        If Not String.IsNullOrEmpty(e.SelectedId) Then
            PopulateMarketingFieldsFromSourceCode(e.SelectedId)
        End If

    End Sub

    Private Sub _mailingid_SearchItemSelected(ByVal sender As Object, ByVal e As UIModeling.Core.SearchItemSelectedEventArgs) Handles _mailingid.SearchItemSelected
        PaymentHelper.SetAppealIDFromMailingID(e.SelectedId, _appealid, Me.GetRequestSecurityContext, Me.GetRequestContext)
    End Sub

    Private Sub _currencyaction_CustomFormConfirmed(ByVal sender As Object, ByVal e As UIModeling.Core.CustomFormConfirmedEventArgs) Handles _currencyaction.CustomFormConfirmed
        Dim model = TryCast(e.Model, CurrencyFormUIModel)
        If model Is Nothing Then Exit Sub

        Me.TRANSACTIONCURRENCYID.Value = model.TRANSACTIONCURRENCYID.Value
        Me.BASEEXCHANGERATEID.Value = model.BASEEXCHANGERATEID.Value
        Me.EXCHANGERATE.Value = model.EXCHANGERATE.Value
    End Sub

    Private Sub _currencyaction_InvokeAction(ByVal sender As Object, ByVal e As UIModeling.Core.ShowCustomFormEventArgs) Handles _currencyaction.InvokeAction
        Dim model = TryCast(e.Model, CurrencyFormUIModel)
        If model Is Nothing Then Exit Sub

        model.AMOUNT.Value = Me.AMOUNT.Value
        model.DATE.Value = Me.STARTDATE.Value
        model.BASECURRENCYID.Value = Me.BASECURRENCYID.Value
        model.TRANSACTIONCURRENCYDROPDOWN.Value = Me.TRANSACTIONCURRENCYID.Value
        model.TRANSACTIONCURRENCYDROPDOWN.Required = True
        model.TRANSACTIONCURRENCYLABEL.Visible = False
        model.BASEEXCHANGERATEID.Value = Me.BASEEXCHANGERATEID.Value
        model.EXCHANGERATE.Value = Me.EXCHANGERATE.Value

    End Sub

    Private Sub _exchangerate_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _exchangerate.ValueChanged
        UpdateBaseAmount()
    End Sub

    Private Sub _transactioncurrencyid_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _transactioncurrencyid.ValueChanged
        'Pass the selected currency everywhere it's needed to ensure proper formatting.
        Dim transactionCurrencyID As Guid = Me.TRANSACTIONCURRENCYID.Value

        For Each installment In Me.INSTALLMENTS.Value
            installment.TRANSACTIONCURRENCYID.Value = transactionCurrencyID
        Next
        Me.INSTALLMENTS.DefaultItem.TRANSACTIONCURRENCYID.Value = transactionCurrencyID

    End Sub


    Private Sub _amount_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _amount.ValueChanged
        If Not Loading Then
            For Each row In _installments.Value
                row.AMOUNT.Value = _amount.Value
            Next
        End If

        UpdateBaseAmount()
    End Sub

    Private Sub _plannedenddate_ValueChanged(ByVal sender As Object, ByVal e As UIModeling.Core.ValueChangedEventArgs) Handles _plannedenddate.ValueChanged
        Me.REVENUESCHEDULEENDDATE.Value = Me.PLANNEDENDDATE.Value
    End Sub
#End Region

    Private Sub _reservationkey_SearchItemSelected(ByVal sender As Object, ByVal e As UIModeling.Core.SearchItemSelectedEventArgs) Handles _reservationkey.SearchItemSelected
        If _initialLoadComplete Then
            If Not Me.RESERVATIONKEY.Value.Equals(Guid.Empty) Then
                Me.RESERVEDOPPORTUNITYIDCHILD.Enabled = True
                Me.RESERVEDOPPORTUNITYIDCHILD.Required = True
                lockHelper.UnlockOpportunity(Me.GetRequestContext)
            Else
                Me.RESERVEDOPPORTUNITYIDCHILD.Enabled = False
                lockHelper.UnlockOpportunity(Me.GetRequestContext)
                Me.RESERVEDOPPORTUNITYIDCHILD.Value = Nothing
                Me.RESERVEDOPPORTUNITYIDCHILD.Required = False
            End If
        End If
    End Sub

    Private Function LockThisChild(ByVal childId As Guid) As Boolean
        'Locks this child in the USR_CHILD_SELECTED_FOR_SPONSORSHIP_LOCK table:
        Dim lockedOk As Boolean = True

        'testing to use a record operation here:
        'c642aa2d-f3f0-4d58-b3f9-a083de9a0009
        Dim operationRequest As New RecordOperationPerformRequest

        Try
            operationRequest.SecurityContext = _securityContext
            operationRequest.ID = childId.ToString()
            'Record Operation name is LockSelectedChild Record Operation
            operationRequest.RecordOperationID = New Guid("c642aa2d-f3f0-4d58-b3f9-a083de9a0009")
            operationRequest.Parameters = New DataFormItem
            With operationRequest.Parameters
                .SetValue("CHANGEAGENTID", Me.GetRequestContext.GetChangeAgentID())
                .SetValue("ID", childId)
            End With

            'the reply is useless, but set it for calling purposes:
            Dim operationReply As New RecordOperationPerformReply
            operationReply = RecordOperationPerform(operationRequest, Me.GetRequestContext)

        Catch ex As Exception
            DisplayErrorMessage(ex.Message)
            lockedOk = False

        End Try

        Return lockedOk
    End Function

    Private Function DeleteChildSelectionLock(ByVal childId As Guid) As Boolean
        'figure out how to call the delete record operation!
        Dim deleteOk As Boolean = False
        Dim deleteOperationRequest As New RecordOperationPerformRequest

        Try
            deleteOperationRequest.SecurityContext = _securityContext
            deleteOperationRequest.ID = childId.ToString()
            'Record Operation name is DeleteChildSelectedLock Record Operation
            deleteOperationRequest.RecordOperationID = New Guid("4868ab3c-0841-4bd1-baed-c2528c48feea")
            deleteOperationRequest.Parameters = New DataFormItem
            With deleteOperationRequest.Parameters
                .SetValue("CHANGEAGENTID", Me.GetRequestContext.GetChangeAgentID())
                .SetValue("ID", childId)
            End With

            Dim deleteOperationReply As RecordOperationPerformReply = RecordOperationPerform(deleteOperationRequest, Me.GetRequestContext)

        Catch ex As Exception
            DisplayErrorMessage(ex.Message)
            deleteOk = False
        End Try

        Return deleteOk
    End Function


    Private Sub ChildrenSelected_ListChanged(ByVal sender As Object, ByVal e As RemovingItemEventArgs)
        ' check if there's a child, or an empty row because the collection field can have an empty row if user escapes out of adding a new one.
        If Not e.Item.Fields("NAME").ValueObject.ToString() = Guid.Empty.ToString() Then
            If DeleteChildSelectionLock(New Guid(e.Item.Fields("NAME").ValueObject.ToString())) Then
                DisplayPrompt("Child lock was removed.")
            End If
        End If
    End Sub

    'Private Sub EnhancedRevenueBatchExtensionHandler_EmbeddedGridCellUpdated(ByVal sender As Object, ByVal e As Blackbaud.AppFx.Browser.Batch.BatchEmbeddedGridCellUpdateEventArgs) Handles Me.CollectionFieldChanged
    '    Try
    '        Dim dfi As DataFormItem = e.EmbeddedEditGridHelper.GetDataFormItemAndTranslationsForRow(e.EmbeddedRow, False)
    '        Dim reply As DataFormLoadReply = Nothing
    '        Dim idDfv As DataFormFieldValue = GetFieldValueObject(dfi, "ID")
    '        If (e.EmbeddedEditGridHelper.ColumnIndexFromFieldID(CATALOGITEM_FIELDID) = e.EmbeddedColumn) Then
    '            Dim catalogItemDfv As DataFormFieldValue = GetFieldValueObject(dfi, CATALOGITEM_FIELDID)
    '            reply = EnhancedRevenueBatchExtensionHandlerHelper.DataFormViewCall(CATALOGITEMPRICEVIEWID, catalogItemDfv.Value.ToString(), Me.BatchID.ToString())
    '            If reply IsNot Nothing Then
    '                Dim price As Decimal = CDec(DataFormHelper.GetFieldValue(reply.DataFormItem, PRICE_FIELDID))
    '                Dim catalogAttributeID As String = DataFormHelper.GetFieldValue(reply.DataFormItem, CATALOGITEMATTRIBUTE_FIELDID).ToString()
    '                If idDfv.Value Is Nothing Then
    '                    SetFieldValue(dfi, "ID", Guid.NewGuid())
    '                End If
    '                SetFieldValue(dfi, PRICE_FIELDID, price)
    '                SetFieldValue(dfi, CATALOGITEMATTRIBUTE_FIELDID, catalogAttributeID)
    '                e.EmbeddedEditGridHelper.UpdateGridRow(dfi, e.EmbeddedRow)
    '            End If

    '        End If
    '    Catch execption As Exception
    '        MessageBox.Show(execption.InnerException.ToString())
    '    End Try
    'End Sub

    Private Sub DisplayErrorMessage(ByVal errorMessage As String)
        DisplayPrompt(errorMessage, UIPromptButtonStyle.Ok)
    End Sub

    Private Sub DisplayPrompt(ByVal message As String, ByVal buttonStyle As UIPromptButtonStyle)
        Me.Prompts.Add(New UIPrompt() With { _
                              .Text = message, _
                              .ButtonStyle = buttonStyle})
    End Sub

    Private Sub DisplayPrompt(ByVal message As String)
        Me.Prompts.Add(New UIPrompt() With { _
                              .Text = message, _
                              .ButtonStyle = UIPromptButtonStyle.Ok})
    End Sub

    Private Sub UnlockSelectedChildren()
        'call the deletelock method for each child in list, if any
        Dim selectedChildren As List(Of SponsorshipAddFormWrappedCHILDRENUIModel) = Me.CHILDREN.Value.ToList()
        For Each child As SponsorshipAddFormWrappedCHILDRENUIModel In selectedChildren
            DeleteChildSelectionLock(New Guid(child.NAME.Value.ToString()))
        Next
    End Sub

    Private Sub CancelPromptCallBack(ByVal sender As Object, ByVal e As UIPromptResponseEventArgs)
        If DirectCast(e.Response, Boolean) Then
            DisplayPrompt("Response was TRUE!")
        Else
            DisplayPrompt("Response was FALSE!")
        End If
    End Sub

    Private Sub ChildrenListVisible(ByVal visible As Boolean)
        'children list panel
        With Me
            .CHILDREN.Visible = visible
            .REMOVESELECTEDCHILD.Visible = visible
        End With
    End Sub

    Private Sub ChildrenListEnableDisable(ByVal enabled As Boolean)
        With Me
            .CHILDREN.Enabled = enabled
            .REMOVESELECTEDCHILD.Enabled = enabled
        End With
    End Sub

    Private Sub ChildrenSelected_AddingNewRow(ByVal sender As Object, ByVal e As ListChangedEventArgs)
        'lock the newly inserted row if this is an Add
        If e.ListChangedType = ListChangedType.ItemAdded Then
            ' check the SourceEvent.PropertyName value for "Value" and get the new child's ID
            ' DirectCast(DirectCast(e,Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs).SourceEvent,
            'Blackbaud.AppFx.UIModeling.Core.UIFieldChangedEventArgs).UIPropertyChangedEventArgs.PropertyName

            'After the user selects a Child, then the type will be different:
            If TypeOf (e) Is Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs Then
                If DirectCast(DirectCast(e, Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs).SourceEvent, Blackbaud.AppFx.UIModeling.Core.UIFieldChangedEventArgs).UIPropertyChangedEventArgs.PropertyName.ToString() = "Value" Then
                    Dim newChildId As Guid = New Guid(DirectCast(DirectCast(e, Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs).SourceEvent, Blackbaud.AppFx.UIModeling.Core.UIFieldChangedEventArgs).UIPropertyChangedEventArgs.NewValue.ToString())
                    LockThisChild(newChildId)
                    If Not Me.REMOVESELECTEDCHILD.Enabled Then
                        Me.REMOVESELECTEDCHILD.Enabled = True
                    End If
                End If
            End If
        Else
            'After the user selects a Child, then the type will be different:
            If TypeOf (e) Is Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs Then
                If DirectCast(DirectCast(e, Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs).SourceEvent, Blackbaud.AppFx.UIModeling.Core.UIFieldChangedEventArgs).UIPropertyChangedEventArgs.PropertyName.ToString() = "Value" Then
                    Dim newChildId As Guid = New Guid(DirectCast(DirectCast(e, Blackbaud.AppFx.UIModeling.Core.CollectionFieldListChangedEventArgs).SourceEvent, Blackbaud.AppFx.UIModeling.Core.UIFieldChangedEventArgs).UIPropertyChangedEventArgs.NewValue.ToString())
                    LockThisChild(newChildId)
                    If Not Me.REMOVESELECTEDCHILD.Enabled Then
                        Me.REMOVESELECTEDCHILD.Enabled = True
                    End If
                End If
            End If
        End If

        If e.ListChangedType = ListChangedType.ItemDeleted Then
            'check the remove button
            Me.REMOVESELECTEDCHILD.Enabled = Me.CHILDREN.HasValue
        End If

    End Sub

    Private Sub ChildrenSelected_ChildInserted(ByVal sender As Object, ByVal e As ValueChangedEventArgs)
        If Not e.NewValue Is Nothing Then
            Dim newChild As SponsorshipAddFormWrappedCHILDRENUIModel = DirectCast(e.NewValue, SponsorshipAddFormWrappedCHILDRENUIModel)
        End If

    End Sub

    Private Sub _addselectedchild_InvokeAction(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs) Handles _addselectedchild.InvokeAction
        'Add the child in the sponsorshipopportunitychild field to the Children list
        If Me.SPONSORSHIPOPPORTUNITYIDCHILD.HasValue Then
            LockThisChild(Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value)
            AddChildToList(Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value())
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value = Nothing
            Me.SPONSORSHIPOPPORTUNITYIDCHILD.UpdateDisplayText(String.Empty)
        End If
    End Sub

    Private Sub AddChildToList()
        'try to lock the child first, to ensure that this user can select the child for sponsorshp:
        If Me.MATCHEDOPPORTUNITYID.HasValue Then
            AddChildToList(New Guid(MATCHEDOPPORTUNITYID.Value.ToString()))
            'Dim newChild As New SponsorshipAddFormWrappedCHILDRENUIModel()
            'newChild.ID = New GuidField("ID") 'Guid.NewGuid()
            'newChild.NAME = New SimpleDataListField(Of Guid) 'Me.SPONSORSHIPOPPORTUNITYIDCHILD
            'newChild.NAME.Value =
            'Me.CHILDREN.Value.Add(newChild)
            'Me.REMOVESELECTEDCHILD.Enabled = True
        End If
    End Sub

    Private Sub AddChildToList(ByVal childId As Guid)
        'try to lock the child first, to ensure that this user can select the child for sponsorshp:
        Dim newChild As New SponsorshipAddFormWrappedCHILDRENUIModel()
        newChild.ID = New GuidField("ID") 'Guid.NewGuid()
        newChild.NAME = New SimpleDataListField(Of Guid) 'Me.SPONSORSHIPOPPORTUNITYIDCHILD
        newChild.LOOKUPID.Value = _lookupid
        newChild.NAME.Value = childId
        Me.CHILDREN.Value.Add(newChild)
        Me.REMOVESELECTEDCHILD.Enabled = True
    End Sub

    Private Sub _sponsorshipopportunityidchild_SearchItemSelected(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.SearchItemSelectedEventArgs) Handles _sponsorshipopportunityidchild.SearchItemSelected
        If Not e.SelectedId = Guid.Empty.ToString() Then
            'turn on the Add Child button
            Me.ADDSELECTEDCHILD.Enabled = True
            'set the lookupid of the selected child:
            _lookupid = e.SelectedRow.Values.ToList().Item(2).ToString()
        Else
            _lookupid = String.Empty
        End If
    End Sub

    Private Sub _sponsorshipopportunityidchild_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs) Handles _sponsorshipopportunityidchild.ValueChanged
        If e.NewValue Is Nothing Then
            Me.ADDSELECTEDCHILD.Enabled = False
        End If
    End Sub
End Class
