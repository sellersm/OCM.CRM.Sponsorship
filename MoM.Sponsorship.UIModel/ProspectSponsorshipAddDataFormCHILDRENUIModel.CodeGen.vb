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
''' Represents the UI model for the 'ProspectSponsorshipAddDataFormCHILDREN' data form
''' </summary>
Partial Public Class [ProspectSponsorshipAddDataFormCHILDRENUIModel]
	Inherits Global.Blackbaud.AppFx.UIModeling.Core.UIModel

#Region "Extensibility methods"

    Partial Private Sub OnCreated()
    End Sub

#End Region

    Private WithEvents _id As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
	'Private WithEvents _name As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
	Private WithEvents _name As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
    Private WithEvents _lookupid As Global.Blackbaud.AppFx.UIModeling.Core.StringField

	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public Sub New()
        MyBase.New()

        _id = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		'_name = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		_name = New Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
        _lookupid = New Global.Blackbaud.AppFx.UIModeling.Core.StringField

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
        _name.Caption = "Child Name"
		_name.Required = True
		_name.SimpleDataListId = New Guid("3eb2bc55-07d8-4b1d-9a48-33883b3fa404")
        Me.Fields.Add(_name)
        '
        '_lookupid
        '
        _lookupid.Name = "LOOKUPID"
        _lookupid.Caption = "Lookup ID"
        Me.Fields.Add(_lookupid)

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
	<System.ComponentModel.Description("Child Name")> _
	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public Property [NAME]() As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid)
		Get
			Return _name
		End Get
		Set(ByVal value As Global.Blackbaud.AppFx.UIModeling.Core.SimpleDataListField(Of Guid))

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
    
End Class
