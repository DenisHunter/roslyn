﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.ObjectModel
Imports System.Text
Imports System.Xml.Linq
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis.VisualBasic.UnitTests
Imports Roslyn.Test.Utilities

Public Class IPEndBlockStatements

    ''' <summary>
    ''' Test1a - Inserts End Function at various places in code
    ''' </summary>
    <WorkItem(899390, "DevDiv/Personal")>
    <Fact>
    Public Sub InsertEndFunctionAtFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Public Sub Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf
        Dim change As String = "End Function"

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.Length, 0),
        .changeType = ChangeType.Insert})
    End Sub

    <Fact>
    Public Sub InsertEndFunctionAtFileMid()
        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Function" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" 1") + 4, 0),
        .changeType = ChangeType.Insert})
    End Sub

    <Fact>
    Public Sub InsertEndFunctionForStatLambda()

        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = Sub()" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Function" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" Sub()") + 8, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test1b - Removes End Function at various places in code
    ''' </summary>
    <WorkItem(899392, "DevDiv/Personal")>
    <Fact>
    Public Sub RemoveEndFunctionFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Public Sub Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Function" & vbCrLf
        Dim change As String = "End Function"

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("End Function"), change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    <Fact>
    Public Sub RemoveEndFunctionFileMid()
        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Function" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" 1") + 4, change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    <Fact>
    Public Sub RemoveEndFunctionStatLambda()
        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = Sub()" & vbCrLf &
            "End Function" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Function" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" Sub()") + 8, change.Length),
        .changeType = ChangeType.Remove})
    End Sub


    ''' <summary>
    ''' Test2b - Removes End Sub at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndSubFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Public Function Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf
        Dim change As String = "End Sub"

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.Length, 0),
        .changeType = ChangeType.Insert})

    End Sub

    <Fact>
    Public Sub InsertEndSubFileMid()
        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Sub" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" 1") + 4, 0),
        .changeType = ChangeType.Insert})

    End Sub

    <Fact>
    Public Sub InsertEndSubStatLambda()

        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = Function()" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Sub" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" Function()") + 13, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test2b - Removes End Sub at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndSubFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Public Function Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Sub" & vbCrLf
        Dim change As String = "End Sub"

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("End Sub"), change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    ''' <summary>
    ''' Test2b - Removes End Sub at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndSubFileMid()
        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = 1" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Sub" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" 1") + 4, change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    ''' <summary>
    ''' Test2b - Removes End Sub at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndSubFileStatLambda()
        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "Dim i = Function()" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Sub" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("= Function()") + 14, change.Length),
        .changeType = ChangeType.Remove})
    End Sub

    ''' <summary>
    ''' Test3a - Inserts End If at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndIfFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "While True" & vbCrLf
        Dim change = "End If" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.Length, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test3a - Inserts End If at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndIfFileMid()
        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "While True" & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End If" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" True") + 7, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test3a - Inserts End If at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndIfStatLambda()
        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "If True Then" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Sub" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" Then") + 7, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test3b - Removes End IF at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndIfFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "While True" & vbCrLf &
            "End If" & vbCrLf
        Dim change = "End If" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("End If"), change.Length),
        .changeType = ChangeType.Remove})
    End Sub

    ''' <summary>
    ''' Test3b - Removes End IF at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndIfFileMid()

        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "While True" & vbCrLf &
            "End If" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End If" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" True") + 7, change.Length),
        .changeType = ChangeType.Remove})
    End Sub

    ''' <summary>
    ''' Test3b - Removes End IF at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndIfStatLambda()

        '===================================================================================================================
        'Scenario3: For a statement lambda
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Sub Foo()" & vbCrLf &
            "If True Then" & vbCrLf &
            "End If" & vbCrLf &
            "End Sub" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End If" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf(" Then") + 7, change.Length),
        .changeType = ChangeType.Remove})
    End Sub

    ''' <summary>
    ''' Test4a - Inserts End Select at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndSelectFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = From el in {1,2} " & vbCrLf &
            "Select el " & vbCrLf
        Dim change = "End Select" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.Length, 0),
        .changeType = ChangeType.Insert})
    End Sub

    ''' <summary>
    ''' Test4a - Inserts End Select at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndSelectFileMid()

        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = From el in {1,2} " & vbCrLf &
            "Select el " & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Select" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("Select el ") + 12, 0),
        .changeType = ChangeType.Insert})

    End Sub

    ''' <summary>
    ''' Test4b - Removes End Select at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndSelectFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = From el in {1,2} " & vbCrLf &
            "Select el " & vbCrLf &
            "End Select" & vbCrLf
        Dim change = "End Select" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("End Select"), change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    ''' <summary>
    ''' Test4b - Removes End Select at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndSelectFileMid()

        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = From el in {1,2} " & vbCrLf &
            "Select el " & vbCrLf &
            "End Select" & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Select" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("Select el ") + 12, change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    ''' <summary>
    ''' Test5a - Inserts End Using at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndUsingFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "With New Integer " & vbCrLf
        Dim change = "End Using" & vbCrLf


        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.Length, 0),
        .changeType = ChangeType.Insert})

    End Sub

    ''' <summary>
    ''' Test5a - Inserts End Using at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndUsingFileMid()

        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "With New Integer " & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Using" & vbCrLf


        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("New Integer ") + 14, 0),
        .changeType = ChangeType.Insert})

    End Sub

    ''' <summary>
    ''' Test5a - Inserts End Using at various places in code
    ''' </summary>
    <Fact>
    Public Sub InsertEndUsingFileMidReplace()

        '===================================================================================================================
        'Scenario3: Replacing In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "With New Integer " & vbCrLf &
            "End With" & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "Using" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("End With") + 4, 4),
        .changeType = ChangeType.Replace})

    End Sub

    ''' <summary>
    ''' Test5b - Removes End Using at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndUsingFileEnd()
        '===================================================================================================================
        'Scenario1: At the end of file
        '===================================================================================================================

        Dim code As String = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "With New Integer " & vbCrLf
        Dim change = "End Using" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("New"), change.Length),
        .changeType = ChangeType.Remove})

    End Sub

    ''' <summary>
    ''' Test5b - Removes End Using at various places in code
    ''' </summary>
    <Fact>
    Public Sub RemoveEndUsingFileMid()

        '===================================================================================================================
        'Scenario2: In the middle of the file
        '===================================================================================================================
        Dim code = "Namespace n1" & vbCrLf &
            "Public Module m1" & vbCrLf &
            "Public Function Foo()" & vbCrLf &
            "Dim i = From el in {1,2} " & vbCrLf &
            "Select el " & vbCrLf &
            "End Select" & vbCrLf &
            "End Function" & vbCrLf &
            "End Module" & vbCrLf &
            "End Namespace" & vbCrLf
        Dim change = "End Select" & vbCrLf

        IncParseAndVerify(New IncParseNode With {
        .oldText = code,
        .changeText = change,
        .changeSpan = New TextSpan(code.IndexOf("Select el ") + 12, change.Length),
        .changeType = ChangeType.Remove})

    End Sub
End Class
