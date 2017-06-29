Imports Blackbaud.AppFx.Server
Imports Blackbaud.AppFx.XmlTypes.DataForms
'Imports Blackbaud.AppFx.Browser

Public Class AddWebSponsorshipOnBoardingInteractionsUIModel

    Private _prospectSponsorship As Boolean
    Private _isValidSponsorship As Boolean
    Private _sponsorshipHelper As SponsorshipHelper
    Private _securityContext As New RequestSecurityContext
    Private _childId As String
    Private _lookupId As String
    Private _childName As String
    Private _isForOneChild As Boolean = False
    Private _message As String = String.Empty



    Private Sub AddWebSponsorshipOnBoardingInteractionsUIModel_Loaded(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
        _prospectSponsorship = False
        _isForOneChild = False
    End Sub

    Private Sub _isprospect_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs) Handles _isprospect.ValueChanged
        'DisplayPrompt("Inside of value changed!!")

        If CBool(e.NewValue) Then
            'checked
            _prospectSponsorship = True
            'prospect sponsorship uses USR_NEWPROSPECTSPONSORSHIPINTERACTIONTYPECODE
            Me.INTERACTIONTYPECODEID.CodeTableName = "USR_NEWPROSPECTSPONSORSHIPINTERACTIONTYPECODE"
            Me.INTERACTIONTYPECODEID.Value = Nothing
            'turn off payment fields:
            'TurnPaymentFieldsOnOff(False)
            Me.ISPROSPECT.Value = True
        Else
            'unchecked
            _prospectSponsorship = False
            'regular sponsorship uses USR_NEWSPONSORSHIPINTERACTIONTYPECODE
            Me.INTERACTIONTYPECODEID.CodeTableName = "USR_NEWSPONSORSHIPINTERACTIONTYPECODE"
            Me.INTERACTIONTYPECODEID.Value = Nothing
            'turn on payment fields:
            'TurnPaymentFieldsOnOff(True)
            Me.ISPROSPECT.Value = False
        End If

        'revalidate the selected sponsor if there is one
        If e.OldValue <> e.NewValue Then
            If Me.SPONSORSHIPCONSTITUENTID.HasValue AndAlso (Not Me.SPONSORSHIPCONSTITUENTID.Equals(Guid.Empty)) Then
                ValidateSponsorConstituency(Me.SPONSORSHIPCONSTITUENTID.Value)
            End If
        End If

        'set the Interaction code table dropdown to the correct codetable:
        Me.INTERACTIONTYPECODEID.ResetDataSource()

    End Sub
    Private Sub ValidateSponsorConstituency(ByVal constituentId As Guid)
        ' call the helper method to validate this sponsor's constituency:
        If _sponsorshipHelper Is Nothing Then
            _sponsorshipHelper = New SponsorshipHelper
        End If

        'check this constituent's constituency code based on the prospect sponsorship flag:
        'if this is a prospect sponsorship, then check for a constituency code of 'Sponsor'
        'if this is a regular sponsorship, then check for a constituency code of 'Prospect Sponsor'
        Dim constituencyCode As String = String.Empty
        _isValidSponsorship = True

        If _prospectSponsorship Then
            constituencyCode = "Sponsor"
        Else
            constituencyCode = "Prospect Sponsor"
        End If

        _isValidSponsorship = _sponsorshipHelper.ValidateSponsorConstituency(constituentId, constituencyCode, _securityContext, Me.GetRequestContext())

        If Not _isValidSponsorship Then
            'inform the user
            'Dim message As String
            If _prospectSponsorship Then
                _message = "The selected sponsor has a Constituency code of 'Sponsor' but you have selected a Prospect Sponsorship! Either uncheck the Prospect Sponsorship or select a different Sponsor."
            Else
                _message = "The selected sponsor has a Constituency code of 'Prospect Sponsor' but you have not selected a Prospect Sponsorship! Either check the Prospect Sponsorship or select a different Sponsor."
            End If

            DisplayErrorMessage(_message)
        End If

    End Sub
    Private Sub DisplayErrorMessage(ByVal errorMessage As String)
        DisplayPrompt(errorMessage, UIPromptButtonStyle.Ok)
    End Sub



    Private Sub _addselectedchild_InvokeAction(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs) Handles _addselectedchild.InvokeAction
        'populate the children grid with matching child
        'but don't set the startdate parameter and do set the child lookupid parameter
        _isForOneChild = True
        GetSponsoredChildren()

        'empty out the child id search field
        Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value = Nothing

        'Add the child in the sponsorshipopportunitychild field to the Children list
        'If Me.SPONSORSHIPOPPORTUNITYIDCHILD.HasValue Then
        '	AddChildToList(_childId, _lookupId, _childName)
        '	Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value = Nothing
        '	'Me.SPONSORSHIPOPPORTUNITYIDCHILD.UpdateDisplayText(String.Empty)
        'End If
    End Sub

    'Private Sub _sponsorshipopportunityidchild_SearchItemSelected(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.SearchItemSelectedEventArgs) Handles _sponsorshipopportunityidchild.SearchItemSelected
    '	If Not e.SelectedId = Guid.Empty.ToString() Then
    '		''DisplayPrompt(e.SelectedRow.Values.ToList().ToString)
    '		''Dim counter As Integer = e.SelectedRow.Values.ToList().Count
    '		'For counter As Integer = 0 To e.SelectedRow.Values.ToList().Count
    '		'	DisplayPrompt(counter.ToString)
    '		'	DisplayPrompt(e.SelectedRow.Values.ToList().Item(counter))
    '		'Next

    '		'DisplayPrompt(_sponsorshipopportunityidchild.SearchDisplayText)

    '		'0 Guid
    '		'1: Name
    '		'2: Lookupid
    '		_childId = e.SelectedRow.Values.ToList().Item(0).ToString()
    '		_childName = e.SelectedRow.Values.ToList().Item(1).ToString()
    '		_lookupId = e.SelectedRow.Values.ToList().Item(2).ToString()

    '		'make sure we got results:
    '		'DisplayPrompt(_childId)
    '		'DisplayPrompt(_childName)
    '		'DisplayPrompt(_lookupId)

    '		'put the found child in the children grid, user can remove if needed:
    '		AddChildToList(_childId, _lookupId, _childName)
    '		Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value = Nothing
    '		'Me.SPONSORSHIPOPPORTUNITYIDCHILD.UpdateDisplayText(String.Empty)

    '		'turn on the Add Child button
    '		'Me.ADDSELECTEDCHILD.Enabled = True
    '		'set the lookupid of the selected child:
    '		'_lookupid = e.SelectedRow.Values.ToList().Item(2).ToString()
    '		'set the programid value that the user used for searching for this child:
    '		'_programid = e.SelectedRow.Values.ToList().Item(9).ToString()
    '	Else
    '		'_lookupid = String.Empty
    '	End If
    'End Sub

    'Private Sub AddChildToList()
    '	If Me.MATCHEDOPPORTUNITYID.HasValue Then
    '		AddChildToList(New Guid(MATCHEDOPPORTUNITYID.Value.ToString()))
    '	End If
    'End Sub

    'Private Sub AddChildToList(ByVal childId As String, ByVal lookupId As String, ByVal childName As String)
    '	Dim newChild As New AddWebSponsorshipOnBoardingInteractionsCHILDRENUIModel()
    '	newChild.ID.Value = New Guid(childId)
    '	newChild.LOOKUPID.Value = lookupId
    '	newChild.NAME.Value = childName
    '	'newChild.SPONSORSHIPDATE.Value = String.Empty
    '	Me.CHILDREN.Value.Add(newChild)
    '	Me.REMOVESELECTEDCHILD.Enabled = True
    ' End Sub

    'Private Sub AddChildToList(ByVal childId As Guid)
    '	'try to lock the child first, to ensure that this user can select the child for sponsorshp:
    '	Dim newChild As New AddWebSponsorshipOnBoardingInteractionsCHILDRENUIModel()
    '	'store the ID of the currently selected programid value
    '	'newChild.ID.Value = New Guid(childId)  'New GuidField("ID") 'Guid.NewGuid()
    '	'newChild.NAME = New SimpleDataListField(Of Guid) 'Me.SPONSORSHIPOPPORTUNITYIDCHILD
    '	'newChild.LOOKUPID.Value = _lookupid
    '	'newChild.NAME.Value = childId
    '	Me.CHILDREN.Value.Add(newChild)
    '	Me.REMOVESELECTEDCHILD.Enabled = True
    'End Sub

    Private Sub AddChildToList(ByVal rowData As DataListResultRow)
        'try to lock the child first, to ensure that this user can select the child for sponsorshp:
        Dim newChild As New AddWebSponsorshipOnBoardingInteractionsCHILDRENUIModel()
        'store the ID of the currently selected programid value
        ' the datalist used to populate Reply variable returns these fields:
        '	CHILDID,	0
        '	CHILDNAME,	1
        '	CHILDLOOKUPID,	2
        '	STARTDATE	3
        '	SPONSORSHIPID 4
        newChild.ID.Value = New Guid(rowData.Values(0))     'New Guid(_programid)  'New GuidField("ID") 'Guid.NewGuid()
        newChild.NAME.Value = rowData.Values(1).ToString()                  'New SimpleDataListField(Of Guid) 'Me.SPONSORSHIPOPPORTUNITYIDCHILD
        newChild.LOOKUPID.Value = rowData.Values(2).ToString()
        newChild.SPONSORSHIPDATE.Value = rowData.Values(3)
        newChild.SPONSORSHIPID.Value = New Guid(rowData.Values(4))
        Me.CHILDREN.Value.Add(newChild)
        Me.REMOVESELECTEDCHILD.Enabled = True
    End Sub


    Private Sub DisplayPrompt(ByVal message As String, ByVal buttonStyle As UIPromptButtonStyle)
        Me.Prompts.Add(New UIPrompt() With {
         .Text = message,
         .ButtonStyle = buttonStyle})
    End Sub

    Private Sub DisplayPrompt(ByVal message As String)
        Me.Prompts.Add(New UIPrompt() With {
         .Text = message,
         .ButtonStyle = UIPromptButtonStyle.Ok})
    End Sub

    Private Sub _findchildren_InvokeAction(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.InvokeActionEventArgs) Handles _findchildren.InvokeAction
        'populate the children grid with matching children:
        'all actively sponsored for the given sponsor with a startdate >= the given filterdate
        'set the global flag to tell the method to set startdate and not set the lookupid
        _isForOneChild = False
        GetSponsoredChildren()
    End Sub

    Private Sub GetSponsoredChildren()
        Dim request As New DataListLoadRequest
        'use the Get Active Web Sponsored Children datalist
        ' returns these fields:
        '	CHILDID,	0
        '	CHILDNAME,	1
        '	CHILDLOOKUPID,	2
        '	STARTDATE	3
        '	SPONSORSHIPID 4
        request.DataListID = New Guid("9f737052-f951-4ad9-a2df-b7c0c079afce") 'New Guid("156c6b80-a895-411f-bebb-198c7a2f9874")
        request.SecurityContext = _securityContext
        request.Parameters = New DataFormItem

        'if this is for all sponsored children, then set startdate and don't set lookupid
        If _isForOneChild = False Then
            With request.Parameters
                .SetValue("SPONSORID", Me.SPONSORSHIPCONSTITUENTID.Value)
                .SetValue("STARTDATE", Me.FILTERDATE.Value)
                .SetValue("LOOKUPID", DBNull.Value)
            End With
        Else
            'if for only one child, then no startdate passed in and lookupid must be set
            With request.Parameters
                .SetValue("SPONSORID", Me.SPONSORSHIPCONSTITUENTID.Value)
                .SetValue("STARTDATE", New Date(1900, 1, 1))
                .SetValue("LOOKUPID", Me.SPONSORSHIPOPPORTUNITYIDCHILD.Value)
            End With
        End If

        Dim reply As DataListLoadReply
        reply = DataListLoad(request, Me.GetRequestContext)

        FillChildrenGridWithListReplyRows(reply)

    End Sub

    Private Sub FillChildrenGridWithListReplyRows(ByVal Reply As Server.DataListLoadReply)
        Try
            If (Reply.Rows IsNot Nothing) Then
                'For Each f As Blackbaud.AppFx.XmlTypes.DataListOutputFieldType In Reply.MetaData.OutputDefinition.OutputFields
                '	If f.IsHidden = True Then
                '		ListView.Columns.Add(f.FieldID, f.Caption, 0)
                '	Else
                '		ListView.Columns.Add(f.FieldID, f.Caption)
                '	End If
                'Next

                For Each row As Blackbaud.AppFx.Server.DataListResultRow In Reply.Rows
                    AddChildToList(row)
                    'ListView.Items.Add(New ListViewItem(row.Values))
                Next

                'ListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                'For Each f As Blackbaud.AppFx.XmlTypes.DataListOutputFieldType In Reply.MetaData.OutputDefinition.OutputFields
                '	If f.IsHidden = True Then
                '		ListView.Columns(f.FieldID).Width = 0
                '	End If
                'Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)

        Finally
            ''Hide hourglass after api call
            'Cursor.Current = Cursors.Default
            'Cursor.Show()
        End Try
    End Sub

    Private Sub AddWebSponsorshipOnBoardingInteractionsUIModel_Saving(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.SavingEventArgs) Handles Me.Saving
        If Me.SPONSORSHIPCONSTITUENTID.HasValue AndAlso (Not Me.SPONSORSHIPCONSTITUENTID.Equals(Guid.Empty)) Then
            ValidateSponsorConstituency(Me.SPONSORSHIPCONSTITUENTID.Value)
            If Not _isValidSponsorship Then
                e.Cancel = True
                e.InvalidReason = _message
            End If
        End If
    End Sub


    Private Sub AddWebSponsorshipOnBoardingInteractionsUIModel_Validating(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValidatingEventArgs) Handles Me.Validating

    End Sub
End Class