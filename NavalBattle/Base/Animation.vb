Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Animation.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

Public Structure Animation

    Private _Frames As IEnumerable(Of Frame)
    Private _Enumerator As IEnumerator(Of Frame)
    Private _Current As Frame

    ''' <summary>
    ''' Cria nova animação
    ''' </summary>
    ''' <param name="frames"> Frames da animação. </param>
    Public Sub New(ByRef frames As IEnumerable(Of Frame))
        Me.New()
        Me.Frames = frames
    End Sub

    ''' <summary>
    ''' Frames da animação.
    ''' </summary>
    ''' <returns> Frames </returns>
    Public Property Frames As IEnumerable(Of Frame)
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
    ''' Troca frame para o proximo frame
    ''' </summary>
    Public Sub NextFrame()
        If Not _Enumerator.MoveNext() Then
            _Enumerator.Reset()
            _Enumerator.MoveNext()
        End If
        _Current = _Enumerator.Current()
    End Sub

    ''' <summary>
    ''' Pega o frame atual
    ''' </summary>
    ''' <returns> Frame atual </returns>
    Public Function Current() As Frame
        Return _Current
    End Function

    ''' <summary>
    ''' Seleciona frame de index especifico
    ''' </summary>
    ''' <param name="index"> Index </param>
    Public Sub SetFrameIndex(ByVal index As Integer)
        If index >= Frames.Count Then
            Throw New IndexOutOfRangeException("The index '" + index.ToString + "' is not in the range 0 to " + Frames.Count.ToString() + ".")
        End If
        _Enumerator.Reset()
        For i = 0 To index
            _Enumerator.MoveNext()
        Next
        _Current = _Enumerator.Current()
    End Sub
End Structure
