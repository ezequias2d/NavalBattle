﻿Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Animation.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Animação composta de Sequência de Frames
''' </summary>
Public Class Animation
    Private _Frames As IEnumerable(Of Frame)
    Private _Enumerator As IEnumerator(Of Frame)
    Private _Frame As Frame

    ''' <summary>
    ''' Cria nova animação
    ''' </summary>
    ''' <param name="frames"> Frames da animação. </param>
    Public Sub New(ByRef frames As ICollection(Of Frame))
        Me.Frames = frames
        Me.LoopEnable = True
    End Sub

    ''' <summary>
    ''' Frames da animação.
    ''' </summary>
    ''' <returns> Frames </returns>
    Public Property Frames As ICollection(Of Frame)
        Get
            If _Frames Is Nothing Then
                _Frames = New LinkedList(Of Frame)
                _Enumerator = _Frames.GetEnumerator()
            End If
            Return _Frames
        End Get
        Set
            If Value Is Nothing Then
                Throw New ArgumentNullException("Frames", "Frames cannot be null.")
            Else
                _Frames = Value
                _Enumerator = _Frames.GetEnumerator()
            End If
        End Set
    End Property

    ''' <summary>
    ''' True para indicar que deve resetar animação sempre que terminar.
    ''' </summary>
    ''' <returns></returns>
    Public Property LoopEnable As Boolean

    ''' <summary>
    ''' Troca frame para o proximo frame
    ''' </summary>
    Public Sub NextFrame()
        Try
            If Not _Enumerator.MoveNext() And LoopEnable Then
                _Enumerator.Reset()
                _Enumerator.MoveNext()
            End If
        Catch ex As InvalidOperationException
            _Enumerator = _Frames.GetEnumerator()
        End Try
        _Frame = _Enumerator.Current()
    End Sub

    ''' <summary>
    ''' Pega o frame atual
    ''' </summary>
    ''' <returns> Frame atual </returns>
    Public Property Current As Frame
        Get
            Return _Frame
        End Get
        Set(value As Frame)
            If Frames.Contains(value) Then
                _Frame = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Seleciona frame de index especifico
    ''' </summary>
    ''' <param name="index"> Index </param>
    Public Sub SetFrameIndex(ByVal index As Integer)
        If index >= Frames.Count Then
            Throw New IndexOutOfRangeException("The index '" + index.ToString + "' is not in the range 0 to " + Frames.Count.ToString() + ".")
        End If

        Try
            _Enumerator.Reset()
        Catch ex As InvalidOperationException
            _Enumerator = _Frames.GetEnumerator()
        End Try

        For i = 0 To index
            _Enumerator.MoveNext()
        Next
        _Frame = _Enumerator.Current()
    End Sub

    Public Sub Reset()
        Try
            _Enumerator.Reset()
        Catch ex As InvalidOperationException
            _Enumerator = _Frames.GetEnumerator()
        End Try

        _Enumerator.MoveNext()
        _Frame = _Enumerator.Current()
    End Sub
End Class
