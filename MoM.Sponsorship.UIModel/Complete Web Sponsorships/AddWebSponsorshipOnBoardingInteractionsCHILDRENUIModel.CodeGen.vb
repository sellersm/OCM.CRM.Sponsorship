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
''' Represents the UI model for the 'AddWebSponsorshipOnBoardingInteractionsCHILDREN' data form
''' </summary>
Partial Public Class [AddWebSponsorshipOnBoardingInteractionsCHILDRENUIModel]
	Inherits Global.Blackbaud.AppFx.UIModeling.Core.UIModel

#Region "Extensibility methods"

    Partial Private Sub OnCreated()
    End Sub

#End Region

    Private WithEvents _id As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
    Private WithEvents _name As Global.Blackbaud.AppFx.UIModeling.Core.StringField
    Private WithEvents _lookupid As Global.Blackbaud.AppFx.UIModeling.Core.StringField
	Private WithEvents _sponsorshipdate As Global.Blackbaud.AppFx.UIModeling.Core.DateField
	Private WithEvents _sponsorshipid As Global.Blackbaud.AppFx.UIModeling.Core.GuidField

	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public Sub New()
        MyBase.New()

        _id = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField
        _name = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
        _lookupid = New Global.Blackbaud.AppFx.UIModeling.Core.StringField
		_sponsorshipdate = New Global.Blackbaud.AppFx.UIModeling.Core.DateField
		_sponsorshipid = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField

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
        Me.Fields.Add(_name)
        '
        '_lookupid
        '
        _lookupid.Name = "LOOKUPID"
        _lookupid.Caption = "Lookup ID"
        Me.Fields.Add(_lookupid)
        '
        '_sponsorshipdate
        '
        _sponsorshipdate.Name = "SPONSORSHIPDATE"
        _sponsorshipdate.Caption = "Start Date"
		Me.Fields.Add(_sponsorshipdate)
		'
		'_sponsorshipid
		'
		_sponsorshipid.Name = "SPONSORSHIPID"
		_sponsorshipid.Caption = "Sponsorship Id"
		_sponsorshipid.Visible = False
		Me.Fields.Add(_sponsorshipid)

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
    Public ReadOnly Property [NAME]() As Global.Blackbaud.AppFx.UIModeling.Core.StringField
        Get
            Return _name
        End Get
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
    ''' Start Date
    ''' </summary>
    <System.ComponentModel.Description("Start Date")> _
    <System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public ReadOnly Property [SPONSORSHIPDATE]() As Global.Blackbaud.AppFx.UIModeling.Core.DateField
        Get
            Return _sponsorshipdate
        End Get
    End Property
	''' <summary>
	''' Sponsorship Id
	''' </summary>
	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
	Public ReadOnly Property [SPONSORSHIPID]() As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
		Get
			Return _sponsorshipid
		End Get
	End Property


End Class