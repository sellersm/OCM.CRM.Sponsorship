Public Class SponsorshipOpportunityChildMoMCustomSearchUIModel

    Private Sub SponsorshipOpportunityChildMoMCustomSearchUIModel_Loaded(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
        If (QUICKSEARCH.Value IsNot Nothing) Then
            If (IsNumeric(QUICKSEARCH.Value.Substring(0, 1))) Then
                LOOKUPID.Value = QUICKSEARCH.Value
            Else
                If (Len(QUICKSEARCH.Value) > 0) Then
                    ParseName(QUICKSEARCH.Value, FIRSTNAME.Value, "", KEYNAME.Value)
                Else
                    KEYNAME.Value = QUICKSEARCH.Value
                End If
            End If
            QUICKSEARCH.Value = String.Empty
        End If
        'default this to True as we're only using it for finding children to sponsor, which is a sole sponsorship:
        Me.RESTRICTFORSOLESPONSORSHIP.Value = True
    End Sub


    Friend Shared Sub ParseName(ByVal fullName As String, ByRef firstName As String, ByRef middleName As String, ByRef lastName As String)
        Dim originalFirstName As String = firstName
        Dim originalMiddleName As String = middleName
        Dim originalLastName As String = lastName
        firstName = String.Empty
        middleName = String.Empty
        lastName = String.Empty

        Dim maxWords As Integer
        Dim fullNameTemp As String
        Dim count As Integer
        Dim lastWord As Integer
        Dim row As Integer
        Dim lastRow As Integer
        Dim lastNameFirst As Boolean
        Dim position As Integer

        Const NAMEBITS As String = "|VON|VAN|DE|DU|DA|DI|LA|LOS|ST|ST.|DER|LE|MC|"

        maxWords = 10
        Dim name(0 To maxWords - 1) As String
        Dim work(0 To maxWords - 1) As String

        fullNameTemp = Trim(fullName)


        If Len(fullNameTemp) > 0 Then
            ' Insert a space after every comma in the string, so the parsing works
            fullNameTemp = Replace(fullNameTemp, ",", ", ")

            If Mid(fullNameTemp, 4, 1) = "." AndAlso Mid(fullNameTemp, 5, 1) <> " " Then
                'If fullNameTemp starts with something like "A.B.Brown" insert a space after the period
                fullNameTemp = Left(fullNameTemp, 4) & " " & Mid(fullNameTemp, 5)
            End If

            If Mid(fullNameTemp, 2, 1) = "." AndAlso Mid(fullNameTemp, 3, 1) <> " " Then
                'if fullNameTemp starts with something like "A.Brown" insert a space after the period
                fullNameTemp = Left(fullNameTemp, 2) & " " & Mid(fullNameTemp, 3)
            End If

            'Parse the full name into individual words, stored in sName()
            count = 1
            Do While (InStr(1, fullNameTemp, " ") > 0)
                name(count) = Trim(Mid(fullNameTemp, 1, InStr(1, fullNameTemp, " ") - 1))
                fullNameTemp = LTrim(Mid$(fullNameTemp, InStr(1, fullNameTemp, " ") + 1))

                count = count + 1

                If count > maxWords Then
                    'Make more room in the array of words
                    maxWords = maxWords + 5
                    ReDim Preserve name(0 To maxWords - 1)
                    ReDim Preserve work(0 To maxWords - 1)
                End If
            Loop

            name(count) = fullNameTemp
            lastWord = count
            lastRow = lastWord

            If lastWord = 1 Then
                lastName = name(1)
            Else
                'work() is a copy of name() after capitalizing and removing all periods and commas
                For count = 1 To lastWord
                    work(count) = UCase(Replace(name(count), ",", String.Empty))
                Next count

                row = 1

                If Right(name(1), 1) = "," Then
                    'i.e. FullName begins with a last name, then a comma, then the rest
                    lastName = name(1)
                    row = 2
                Else
                    If Right(name(2), 1) = "," Then
                        'Second word ends with a comma, perhaps:  "John Smith, Jr."
                        lastNameFirst = True
                        If (lastWord = 3) OrElse (lastWord = 4) Then
                            'We may have "John Smith, Jr." or "John Smith, MA, Ph.D." on the one hand,
                            ' or "Van Gilder, Max" or "Van Gilder, Dr. Max" on the other.                            
                        End If

                        If lastNameFirst Then
                            lastName = name(1) & " " & LTrim(name(2))
                            row = 3
                        End If
                    End If
                End If

                'Set last name if it is empty
                If Len(lastName) = 0 Then
                    lastName = name(lastRow)
                    lastRow = lastRow - 1
                End If

                'If LastName ends with a comma, remove it
                If Right(lastName, 1) = "," Then
                    lastName = Mid(lastName, 1, Len(lastName) - 1)
                    If Len(lastName) = 0 Then
                        lastName = name(lastRow)
                        lastRow = lastRow - 1
                    End If
                End If

                For count = row To lastRow
                    firstName = firstName & " " & name(count)
                Next
                firstName = LTrim(firstName)

                If Right(firstName, 1) = "," Then
                    firstName = Mid(firstName, 1, Len(firstName) - 1)
                End If

                If (Right(firstName, 1) = ".") AndAlso (Left(Right(firstName, 3), 1) = ".") Then
                    If Len(firstName) >= 4 Then
                        firstName = Left(firstName, Len(firstName) - 2) & " " & Mid(firstName, Len(firstName) - 1)
                    End If
                End If

                If Mid(firstName, 4, 1) = "." AndAlso Mid(firstName, 5, 1) <> " " Then
                    'Firstname starts with something like "A.B.Brown"
                    ' insert a space to create "A.B. Brown"
                    firstName = Left(firstName, 4) & " " & Mid(firstName, 5)
                End If

                If Mid(firstName, 2, 1) = "." AndAlso Mid(firstName, 3, 1) <> " " Then
                    'Firstname starts with something like "A.Brown"
                    ' insert a space to create "A. Brown"
                    firstName = Left(firstName, 2) & " " & Mid(firstName, 3)
                End If

                Do While InStr(NAMEBITS, "|" & UCase(Mid(firstName, InStrRev(firstName, " ") + 1)) & "|") > 0
                    position = InStrRev(firstName, " ")
                    lastName = Mid(firstName, position + 1) & " " & lastName
                    If position > 0 Then
                        firstName = Mid(firstName, 1, position - 1)
                    Else
                        firstName = String.Empty
                    End If
                Loop

                If InStr(Trim(firstName), " ") > 0 Then
                    position = InStrRev(Trim(firstName), " ")
                    middleName = Mid(firstName, position + 1)
                    firstName = Trim(Mid(firstName, 1, position))
                End If

                'If the last name starts with a single character followed by an initial,
                'and there is no middle initial yet, assume that the single character
                'is the middle initial:  "John J.Smith"
                If Mid(lastName, 2, 1) = "." AndAlso Len(middleName) = 0 Then
                    middleName = Mid(lastName, 1, 2)
                    lastName = Mid(lastName, 3)
                End If

            End If

            firstName = Trim(firstName)
            middleName = Trim(middleName)
            lastName = Trim(lastName)

        End If

        Erase name, work

        'Reset the parameters to the original values if they were not set by parsing fullName
        If Len(firstName) = 0 AndAlso (originalFirstName IsNot Nothing) Then
            firstName = originalFirstName
        End If
        If Len(middleName) = 0 AndAlso (originalMiddleName IsNot Nothing) Then
            middleName = originalMiddleName
        End If
        If Len(lastName) = 0 AndAlso (originalLastName IsNot Nothing) Then
            lastName = originalLastName
        End If
    End Sub


End Class