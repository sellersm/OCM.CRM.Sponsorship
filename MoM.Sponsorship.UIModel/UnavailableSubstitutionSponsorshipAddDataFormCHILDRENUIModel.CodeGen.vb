﻿Option Strict On
Option Explicit On
Option Infer On

'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by BBUIModelLibrary
'     Version:  2.93.2034.0
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------
''' <summary>
''' Represents the UI model for the 'UnavailableSubstitutionSponsorshipAddDataFormCHILDREN' data form
''' </summary>
Partial Public Class [UnavailableSubstitutionSponsorshipAddDataFormCHILDRENUIModel]
	Inherits Global.Blackbaud.AppFx.UIModeling.Core.UIModel

#Region "Extensibility methods"

	Partial Private Sub OnCreated()
	End Sub

#End Region

	Private WithEvents _id As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
	Private WithEvents _name As Global.Blackbaud.AppFx.UIModeling.Core.StringField
	Private WithEvents _lookupid As Global.Blackbaud.AppFx.UIModeling.Core.StringField
	Private WithEvents _transferchildname As Global.Blackbaud.AppFx.UIModeling.Core.StringField
	Private WithEvents _transferlookupid As Global.Blackbaud.AppFx.UIModeling.Core.StringField
	Private WithEvents _transferchildid As Global.Blackbaud.AppFx.UIModeling.Core.GuidField

	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public Sub New()
		MyBase.New()

		_id = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		_name = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
		_lookupid = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
		_transferchildname = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
		_transferlookupid = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
		_transferchildid = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField

		'
		'_id
		'
		_id.Name = "ID"
		_id.Caption = "ID"
		_id.Visible = False
		Me.Fields.Add(_id)
		'
		'_name
		'
		_name.Name = "NAME"
		_name.Caption = "Unavail. Child"
		_name.Required = True
		Me.Fields.Add(_name)
		'
		'_lookupid
		'
		_lookupid.Name = "LOOKUPID"
		_lookupid.Caption = "Lookup ID"
		Me.Fields.Add(_lookupid)
		'
		'_transferchildname
		'
		_transferchildname.Name = "TRANSFERCHILDNAME"
		_transferchildname.Caption = "Transfer Child"
		Me.Fields.Add(_transferchildname)
		'
		'_transferlookupid
		'
		_transferlookupid.Name = "TRANSFERLOOKUPID"
		_transferlookupid.Caption = "Transfer Lookup ID"
		Me.Fields.Add(_transferlookupid)
		'
		'_transferchildid
		'
		_transferchildid.Name = "TRANSFERCHILDID"
		_transferchildid.Caption = "TRANSFERCHILDID"
		_transferchildid.Visible = False
		Me.Fields.Add(_transferchildid)

		OnCreated()

	End Sub

	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public ReadOnly Property [ID]() As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		Get
			Return _id
		End Get
	End Property

	''' <summary>
	''' Child Name
	''' </summary>
	<System.ComponentModel.Description("Unavailable Child")> _
	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public Property [NAME]() As Global.Blackbaud.AppFx.UIModeling.Core.StringField	 'Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
		Get
			Return _name
		End Get
		Set(ByVal value As Global.Blackbaud.AppFx.UIModeling.Core.StringField)

		End Set
	End Property

	''' <summary>
	''' Lookup ID
	''' </summary>
	<System.ComponentModel.Description("Lookup ID")> _
	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public ReadOnly Property [LOOKUPID]() As Global.Blackbaud.AppFx.UIModeling.Core.StringField
		Get
			Return _lookupid
		End Get
	End Property

	''' <summary>
	''' Sub Child
	''' </summary>
	'<System.ComponentModel.Description("Sub Child")> _
	'<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	'Public ReadOnly Property [SUBSTITUTECHILDNAME]() As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
	'    Get
	'        Return _transferchildname
	'    End Get
	'End Property
	<System.ComponentModel.Description("Transfer Child")> _
	  <System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public Property [TRANSFERCHILDNAME]() As Global.Blackbaud.AppFx.UIModeling.Core.StringField	'Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
		Get
			Return _transferchildname
		End Get
		Set(ByVal value As Global.Blackbaud.AppFx.UIModeling.Core.StringField)

		End Set
	End Property
	''' <summary>
	''' Sub Lookup ID
	''' </summary>
	<System.ComponentModel.Description("Transfer Lookup ID")> _
	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public ReadOnly Property [TRANSFERLOOKUPID]() As Global.Blackbaud.AppFx.UIModeling.Core.StringField
		Get
			Return _transferlookupid
		End Get
	End Property


	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public ReadOnly Property [TRANSFERCHILDID]() As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		Get
			Return _transferchildid
		End Get
	End Property

End Class
