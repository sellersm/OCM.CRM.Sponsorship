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
''' Represents the UI model for the 'UnavailableSubstitutionSponsorshipAddDataFormINSTALLMENTS' data form
''' </summary>
Partial Public Class [UnavailableSubstitutionSponsorshipAddDataFormINSTALLMENTSUIModel]
	Inherits Global.Blackbaud.AppFx.UIModeling.Core.UIModel

#Region "Extensibility methods"

    Partial Private Sub OnCreated()
    End Sub

#End Region

    Private WithEvents _date As Global.Blackbaud.AppFx.UIModeling.Core.DateField
    Private WithEvents _amount As Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
    Private WithEvents _transactioncurrencyid As Global.Blackbaud.AppFx.UIModeling.Core.GuidField

	<System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public Sub New()
        MyBase.New()

        _date = New Global.Blackbaud.AppFx.UIModeling.Core.DateField
        _amount = New Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
        _transactioncurrencyid = New Global.Blackbaud.AppFx.UIModeling.Core.GuidField

        '
        '_date
        '
        _date.Name = "DATE"
        _date.Caption = "Date"
        _date.Required = True
        Me.Fields.Add(_date)
        '
        '_amount
        '
        _amount.Name = "AMOUNT"
        _amount.Caption = "Amount"
        _amount.Required = True
        _amount.CurrencyFieldId = "TRANSACTIONCURRENCYID"
        Me.Fields.Add(_amount)
        '
        '_transactioncurrencyid
        '
        _transactioncurrencyid.Name = "TRANSACTIONCURRENCYID"
        _transactioncurrencyid.Caption = "Transaction currency"
        _transactioncurrencyid.Visible = False
        Me.Fields.Add(_transactioncurrencyid)

		OnCreated()

    End Sub
    
    ''' <summary>
    ''' Date
    ''' </summary>
    <System.ComponentModel.Description("Date")> _
    <System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public ReadOnly Property [DATE]() As Global.Blackbaud.AppFx.UIModeling.Core.DateField
        Get
            Return _date
        End Get
    End Property
    
    ''' <summary>
    ''' Amount
    ''' </summary>
    <System.ComponentModel.Description("Amount")> _
    <System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public ReadOnly Property [AMOUNT]() As Global.Blackbaud.AppFx.UIModeling.Core.MoneyField
        Get
            Return _amount
        End Get
    End Property
    
    ''' <summary>
    ''' Transaction currency
    ''' </summary>
    <System.ComponentModel.Description("Transaction currency")> _
    <System.CodeDom.Compiler.GeneratedCodeAttribute("BBUIModelLibrary", "2.93.2034.0")> _
    Public ReadOnly Property [TRANSACTIONCURRENCYID]() As Global.Blackbaud.AppFx.UIModeling.Core.GuidField
        Get
            Return _transactioncurrencyid
        End Get
    End Property
    
End Class
