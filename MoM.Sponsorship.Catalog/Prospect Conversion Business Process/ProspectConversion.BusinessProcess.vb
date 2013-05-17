Imports System.Text
Imports Blackbaud.AppFx.Server
Imports System.Data.SqlClient

Public NotInheritable Class ProspectConversionBusinessProcess
	Inherits AppCatalog.AppBusinessProcess

	' assumes the business process id value is E893C10B-4E01-496F-9657-0E64CF1B895F

	Private Const outputTablePrefix As String = "USR_PROSPECTSPONSOR_CONVERSION_BUSINESSPROCESS"

	Public Overrides Function StartBusinessProcess() As Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult
		Dim ProcessResults As New Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult

		Dim Transaction As Data.SqlClient.SqlTransaction = Nothing

		Me.UpdateProcessStatus("Processing prospect conversions...")
		' Execute the command to populate the table

		Dim outputTableName As String = CreateOutputTable(outputTablePrefix, "SUCCESS", OutputTableColumns().ToArray())
		'Dim exceptionTableName As String = CreateOutputTable(String.Concat(outputTablePrefix, exceptionTablePrefix), "Exception", ExceptionTableColumns().ToArray())

		Dim successParam As SqlParameter
		Dim exceptionParam As SqlParameter

		Using Connection As SqlConnection = New SqlConnection(Me.RequestContext.AppDBConnectionString)
			Connection.Open()
			Using Command As SqlCommand = Connection.CreateCommand()
				With Command
					Try
						Transaction = Connection.BeginTransaction
						.Transaction = Transaction

						.CommandType = CommandType.StoredProcedure
						.CommandText = "dbo.USR_USP_PROSPECT_CONVERSION_BUSINESSPROCESS"
						.CommandTimeout = Me.ProcessCommandTimeout
						.Parameters.AddWithValue("changeAgentID", DBNull.Value)				' The sproc will get the Change Agent ID if we pass in null
						.Parameters.AddWithValue("outputTableName", outputTableName)		' The table to insert the results of conversion process
						'.Parameters.AddWithValue("exceptionTableName", exceptionTableName)	' The table to insert exceptions and kids put on the waiting list

						successParam = New SqlParameter("successCount", SqlDbType.Int)		' The number of kids registered
						successParam.Direction = ParameterDirection.Output
						.Parameters.Add(successParam)

						exceptionParam = New SqlParameter("exceptionCount", SqlDbType.Int)	' The number of failed conversions
						exceptionParam.Direction = ParameterDirection.Output
						.Parameters.Add(exceptionParam)

						.ExecuteNonQuery()

						Transaction.Commit()

						If successParam.Value IsNot Nothing AndAlso IsNumeric(successParam.Value) Then
							ProcessResults.NumberSuccessfullyProcessed = CInt(successParam.Value)
						Else
							ProcessResults.NumberSuccessfullyProcessed = 7777777	' We didn't get a value.  Unfortunately, I can't set this to -1, so I picked a value that we shouldn't ever have.
						End If

						If exceptionParam.Value IsNot Nothing AndAlso IsNumeric(exceptionParam.Value) Then
							ProcessResults.NumberOfExceptions = CInt(exceptionParam.Value)
						Else
							ProcessResults.NumberOfExceptions = 7777777				' We didn't get a value.  Unfortunately, I can't set this to -1, so I picked a value that we shouldn't ever have.
						End If

					Catch ex As Exception
						Transaction.Rollback()

						Throw ex
					End Try


				End With

			End Using
		End Using

		Return ProcessResults
	End Function

	'I think it would be good to have an output table of the sponsorships that were converted and the errors, 
	'so that we can create a query view to find the ones that were converted over a period of time.  It can be a 
	'single table with a column for the status (success / failure) and a column for an error message for exceptions 
	'or it can be two separate tables – one for success and one for exceptions.  
	'Whatever you prefer.  The query view allows people to step through the sponsorships that had exceptions.
	Private Function OutputTableColumns() As List(Of AppCatalog.TableColumn)
		'
		'- store in table:
		' - sponsorlookupid
		' - NUMBERSPONSORSHIPCONVERSIONS
		' - conversiondate 
		' - conversionresult: Success/Fail
		' - exceptionmessage: NULLABLE only populated when status = Fail

		Dim columns As New List(Of AppCatalog.TableColumn)
		columns.Add(New AppCatalog.TableColumn("SPONSORLOOKUPID", SqlDbType.NVarChar))
		columns.Add(New AppCatalog.TableColumn("NUMBERSPONSORSHIPCONVERSIONS", SqlDbType.Int))
		columns.Add(New AppCatalog.TableColumn("CONVERSIONDATE", SqlDbType.Date))
		columns.Add(New AppCatalog.TableColumn("CONVERSIONRESULT", SqlDbType.NVarChar, 7))
		columns.Add(New AppCatalog.TableColumn("EXCEPTIONMESSAGE", SqlDbType.NVarChar, 4000))
		Return columns

	End Function

	'Private Function ExceptionTableColumns() As List(Of AppCatalog.TableColumn)

	'	Dim columns As New List(Of AppCatalog.TableColumn)
	'	columns.Add(New AppCatalog.TableColumn("CHILDPROJECTID", SqlDbType.UniqueIdentifier))
	'	columns.Add(New AppCatalog.TableColumn("COUNTRYID", SqlDbType.UniqueIdentifier))
	'	columns.Add(New AppCatalog.TableColumn("SPONSORSHIPOPPORTUNITYCHILDID", SqlDbType.UniqueIdentifier))
	'	columns.Add(New AppCatalog.TableColumn("ERRORMESSAGE", SqlDbType.NVarChar, 255))
	'	Return columns

	'End Function

	'optional overrides
	'Public Overrides Sub CheckForSimultaneousRuns()
	'    MyBase.CheckForSimultaneousRuns()
	'End Sub

	'Public Overrides Function GetAppLockNameForParameterSet() As String
	'    Return MyBase.GetAppLockNameForParameterSet()
	'End Function

End Class
