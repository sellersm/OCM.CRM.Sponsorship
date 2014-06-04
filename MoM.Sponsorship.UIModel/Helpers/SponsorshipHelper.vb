Imports Blackbaud.AppFx.UIModeling.Core.Utilities
Imports Blackbaud.AppFx.Server
Imports Blackbaud.AppFx.XmlTypes
Imports Blackbaud.AppFx.XmlTypes.DataForms
Imports Blackbaud.AppFx.Fundraising.UIModel
Imports System.Globalization
Imports Blackbaud.AppFx.Sponsorship.UIModel

Public Class SponsorshipHelper

	Public Function ValidateSponsorConstituency(ByVal constituentId As Guid, ByVal constituencyCode As String, _
			   ByVal securityContext As RequestSecurityContext, ByVal requestContext As RequestContext) As Boolean
		'4436ed8a-279b-437e-9ba3-d0a01c3106f4 is the Guid of the datalist to use
		'check this constituent's constituency code based on the prospect sponsorship flag:
		'if this is a prospect sponsorship, then check for a constituency code of 'Sponsor'
		'if this is a regular sponsorship, then check for a constituency code of 'Prospect Sponsor'
		Dim isValidSponsorship As Boolean = True

		Dim request As New DataListLoadRequest
		'use the MoM Custom datalist:
		request.DataListID = New Guid("4436ed8a-279b-437e-9ba3-d0a01c3106f4")
		request.SecurityContext = securityContext
		request.Parameters = New DataFormItem
		With request.Parameters
			.SetValue("CONSTITUENTID", constituentId)
			.SetValue("CONSTITUENCYCODE", constituencyCode)
		End With
		Dim reply As DataListLoadReply
		reply = DataListLoad(request, requestContext)

		If Not reply.Rows Is Nothing AndAlso reply.Rows.Length > 0 Then
			If Not String.IsNullOrEmpty(reply.Rows(0).Values(0)) Then
				Dim hasCode As Int32 = reply.Rows(0).Values(0)
				'the datalist returns 0 if constituent doesn't have the given code
				isValidSponsorship = (hasCode.Equals(0)) 'CBool(reply.Rows(0).Values(0))
			End If
		End If

		Return isValidSponsorship

	End Function

	Public Function GetFundraiserForAppealId(ByVal appealId As Guid, ByVal securityContext As RequestSecurityContext, ByVal requestContext As RequestContext) As Guid
		'this is where we'll try to find the Fundraiser constituentID for this Appeal:
		'591cb645-5d30-4892-afbf-39d900f00f11 id of the datalist:
		Dim constituentId As Guid
		Dim request As New DataListLoadRequest
		If Not appealId.Equals(Guid.Empty) Then
			'use the MoM Custom datalist:
			request.DataListID = New Guid("591cb645-5d30-4892-afbf-39d900f00f11")
			request.SecurityContext = securityContext
			request.Parameters = New DataFormItem
			With request.Parameters
				.SetValue("APPEALID", appealId)
			End With
			Dim reply As DataListLoadReply
			reply = DataListLoad(request, requestContext)

			If Not reply.Rows Is Nothing AndAlso reply.Rows.Length > 0 Then
				If Not String.IsNullOrEmpty(reply.Rows(0).Values(0)) Then
					constituentId = New Guid(reply.Rows(0).Values(0))
					'Me.FUNDRAISERID.Value = constituentId
					'Me.FUNDRAISERID.UpdateDisplayText()
				End If
			End If
		End If

		Return constituentId

	End Function

	' Memphis 9/12/12: Not currently needed...
	'Private Function GetConstituencyCodeId(ByVal constituencyCode As String) As Guid
	'	'8e3dfe81-7a9d-41f5-b73a-c24990d41072 is the datalist to use:
	'	Dim constituencyCodeId As Guid = Guid.Empty
	'	Dim request As New DataListLoadRequest
	'	'use the MoM Custom datalist:
	'	request.DataListID = New Guid("8e3dfe81-7a9d-41f5-b73a-c24990d41072")
	'	request.SecurityContext = _securityContext
	'	request.Parameters = New DataFormItem
	'	With request.Parameters
	'		.SetValue("CONSTITUENCYCODE", constituencyCode)
	'		'.SetValue("CONSTITUENCYCODEID", constituencyCodeId)	  ' be sure we have a constituent!!
	'	End With
	'	Dim reply As DataListLoadReply
	'	reply = DataListLoad(request, Me.GetRequestContext)

	'	If Not reply.Rows Is Nothing AndAlso reply.Rows.Length > 0 Then
	'		If Not String.IsNullOrEmpty(reply.Rows(0).Values(0)) Then
	'			constituencyCodeId = New Guid(reply.Rows(0).Values(0))
	'		End If
	'	End If

	'	Return constituencyCodeId

	'End Function

End Class
