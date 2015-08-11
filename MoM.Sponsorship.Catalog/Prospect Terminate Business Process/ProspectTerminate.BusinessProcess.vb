Imports System.Text
Imports Blackbaud.AppFx.Server
Imports System.Data.SqlClient

Public NotInheritable Class ProspectTerminateBusinessProcess
	Inherits AppCatalog.AppBusinessProcess

	Private _idSetID As Guid = Guid.Empty

	Private Const outputTablePrefix As String = "USR_PROSPECTTERMINATE_BUSINESSPROCESS"

	Public Overrides Function StartBusinessProcess() As Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult
		Dim ProcessResults As New Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult

		Dim Transaction As Data.SqlClient.SqlTransaction = Nothing

		'Dim campaignType As Integer = RequestArgs.DataItem.Values.Item("CAMPAIGNTYPE").Value

		Me.UpdateProcessStatus("Processing Prospect Terminates...")
		' Execute the command to populate the table

		Dim outputTableName As String = CreateOutputTable(outputTablePrefix, "_RESULTS", OutputTableColumns().ToArray())
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
						.CommandText = "dbo.USR_USP_PROSPECTTERMINATE_BUSINESSPROCESS"
						.CommandTimeout = Me.ProcessCommandTimeout
						.Parameters.AddWithValue("idSetRegisterID", _idSetID)
						.Parameters.AddWithValue("changeAgentID", DBNull.Value)				' The sproc will get the Change Agent ID if we pass in null
						'.Parameters.AddWithValue("outputTableName", outputTableName)		' The table to insert the results of conversion process
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

	'Validate gets called first and if all goes well, it gets our parameters too.
	Public Overrides Sub Validate()
		MyBase.Validate()
		' Get our business process parameters
		GetParameters(RequestArgs.ParameterSetID, Me.RequestContext)
		'GetParameters(RequestArgs.DataItem.Values.Item("ID").Value, Me.RequestContext)

		'If _campaignType = Nothing Then
		'	Throw New Exception("No parameters found with the given parameter")
		'End If
	End Sub


	Private Function OutputTableColumns() As List(Of AppCatalog.TableColumn)
		'
		'- store in table:

		Dim columns As New List(Of AppCatalog.TableColumn)
		columns.Add(New AppCatalog.TableColumn("SPONSORTERMINATEDATE", SqlDbType.Date))
		columns.Add(New AppCatalog.TableColumn("SPONSORTERMINATETYPE", SqlDbType.TinyInt))
		'columns.Add(New AppCatalog.TableColumn("D4INTERACTIONSTATUS", SqlDbType.NVarChar, 50))
		columns.Add(New AppCatalog.TableColumn("TERMINATECOUNT", SqlDbType.Int))
		columns.Add(New AppCatalog.TableColumn("EXCEPTIONCOUNT", SqlDbType.Int))
		Return columns

	End Function
	'optional overrides
	'Public Overrides Sub CheckForSimultaneousRuns()
	'    MyBase.CheckForSimultaneousRuns()
	'End Sub

	'Public Overrides Function GetAppLockNameForParameterSet() As String
	'    Return MyBase.GetAppLockNameForParameterSet()
	'End Function


	Public Sub GetParameters(ByVal parameterSetID As Guid, ByRef requestContext As RequestContext)
		Using con As SqlConnection = New SqlConnection(requestContext.AppDBConnectionString)
			Using command As SqlCommand = con.CreateCommand()
				Try
					command.CommandText = "USR_USP_PROSPECTTERMINATEPROCESS_GETPARAMETERS"
					command.CommandType = CommandType.StoredProcedure
					command.Parameters.AddWithValue("@ID", parameterSetID)
					con.Open()
					Using reader As SqlDataReader = command.ExecuteReader()
						reader.Read()
						_idSetID = reader.GetValue(reader.GetOrdinal("IDSETREGISTERID"))
						reader.Close()
					End Using
					con.Close()
				Catch ex As Exception
					Throw New Exception(ex.Message + " " + "Unable to get parameter set for the given Id")
				End Try
			End Using
		End Using
	End Sub



End Class
