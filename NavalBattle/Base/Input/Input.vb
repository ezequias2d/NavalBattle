Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Input.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Entrada de controles, teclado ou mouse atraves de uma camada de abstração de eixos
''' </summary>
Public Class Input
    Implements IUpdate

    ''' <summary>
    ''' Tabela com valores do eixo(Single)
    ''' Eixo pode ir de -1 a 1
    ''' (Valor, Se clicado agora, Matem seguro)
    ''' </summary>
    Private axisTab As IDictionary(Of Axis, (Single, Boolean, Boolean, HashSet(Of IAxis)))

    Private equalityComparer As IEqualityComparer(Of IAxis)

    ''' <summary>
    ''' Intancia da classe 
    ''' </summary>
    Private Shared _Input As Input

    ''' <summary>
    ''' Instancia da classe
    ''' </summary>
    ''' <returns> Instancia </returns>
    Public Shared ReadOnly Property Instance As Input
        Get
            If _Input Is Nothing Then
                _Input = New Input()
            End If
            Return _Input
        End Get
    End Property

    ''' <summary>
    ''' Ler um Axis especifico
    ''' </summary>
    ''' <param name="ax"> Axis </param>
    ''' <returns> Valor do Axis atual </returns>
    Public Shared Function ReadAxis(ByVal ax As Axis) As Single
        Return Instance.axisTab(ax).Item1
    End Function

    Public Shared Function ReadAxisClick(ByVal ax As Axis) As Integer
        Return Instance.axisTab(ax).Item2 * Math.Sign(Instance.axisTab(ax).Item1)
    End Function

    Public Shared Property DebugMode As Boolean


    Private Sub New()
        equalityComparer = New AxisEqualityComparer()
        axisTab = New Dictionary(Of Axis, (Single, Boolean, Boolean, HashSet(Of IAxis)))
        UpdateEnable = True
        DebugMode = False

        resetAxisTab()

        ' Configura todos os Axis padrão

        Add(New AxisKeyboard(Axis.Horizontal, Keys.A, Keys.D))
        Add(New AxisKeyboard(Axis.Horizontal, Keys.Left, Keys.Right))
        Add(New AxisGamepad(Axis.Horizontal, GamePadButton.XAxisLeft, GamePadButton.None))
        Add(New AxisGamepad(Axis.Horizontal, GamePadButton.DPadX, GamePadButton.DPadX))

        Add(New AxisKeyboard(Axis.Vertical, Keys.W, Keys.S))
        Add(New AxisKeyboard(Axis.Vertical, Keys.Up, Keys.Down))
        Add(New AxisGamepad(Axis.Vertical, GamePadButton.None, GamePadButton.YAxisLeft))
        Add(New AxisGamepad(Axis.Vertical, GamePadButton.DPadY, GamePadButton.DPadY))

        Add(New AxisKeyboard(Axis.Fire0, Keys.B, Keys.None))
        Add(New AxisKeyboard(Axis.Fire0, Keys.Z, Keys.None))
        Add(New AxisMouse(Axis.Fire0, MouseButton.Left, MouseButton.None))
        Add(New AxisGamepad(Axis.Fire0, GamePadButton.A, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Fire1, Keys.N, Keys.None))
        Add(New AxisKeyboard(Axis.Fire1, Keys.X, Keys.None))
        Add(New AxisMouse(Axis.Fire1, MouseButton.Right, MouseButton.None))
        Add(New AxisGamepad(Axis.Fire1, GamePadButton.B, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Fire2, Keys.M, Keys.None))
        Add(New AxisKeyboard(Axis.Fire2, Keys.C, Keys.None))
        Add(New AxisMouse(Axis.Fire2, MouseButton.Middle, MouseButton.None))
        Add(New AxisGamepad(Axis.Fire2, GamePadButton.Y, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Jump, Keys.Space, Keys.None))
        Add(New AxisGamepad(Axis.Jump, GamePadButton.X, GamePadButton.None))


        Add(New AxisKeyboard(Axis.Submit, Keys.Enter, Keys.None))
        Add(New AxisGamepad(Axis.Submit, GamePadButton.A, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Cancel, Keys.Back, Keys.None))
        Add(New AxisGamepad(Axis.Cancel, GamePadButton.B, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Start, Keys.Enter, Keys.None))
        Add(New AxisGamepad(Axis.Start, GamePadButton.Start, GamePadButton.None))

        Add(New AxisKeyboard(Axis.Options, Keys.Escape, Keys.None))
        Add(New AxisGamepad(Axis.Options, GamePadButton.Back, GamePadButton.None))

        Add(New AxisMouse(Axis.MouseX, MouseButton.X, MouseButton.None))
        Add(New AxisGamepad(Axis.MouseX, GamePadButton.XAxisLeft, GamePadButton.None))

        Add(New AxisMouse(Axis.MouseY, MouseButton.Y, MouseButton.None))
        Add(New AxisGamepad(Axis.MouseY, GamePadButton.YAxisLeft, GamePadButton.None))
    End Sub

    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable

    ''' <summary>
    ''' Update do Input
    ''' 
    ''' Atualiza AxisTab
    ''' </summary>
    ''' <param name="gameTime"></param>
    Public Sub Update(gameTime As GameTime) Implements IUpdate.Update
        resetAxisTab()
    End Sub

    ''' <summary>
    ''' Adiciona novo IAxis
    ''' </summary>
    ''' <param name="axisToAdd"> IAxis para adicionar </param>
    ''' <returns> Se adicionou </returns>
    Public Function Add(ByRef axisToAdd As IAxis) As Boolean
        If Not axisTab.ContainsKey(axisToAdd.Axis) Then
            changeValueAxisTab(axisToAdd.Axis)
        End If
        Return axisTab(axisToAdd.Axis).Item4.Add(axisToAdd)
    End Function

    ''' <summary>
    ''' Remove IAxis, se este inserido está
    ''' </summary>
    ''' <param name="axisToRemove"> Axis para remover</param>
    ''' <returns> Se removeu </returns>
    Public Function Remove(ByRef axisToRemove As IAxis) As Boolean
        Return axisTab(axisToRemove.Axis).Item4.Remove(axisToRemove)
    End Function

    ''' <summary>
    ''' Pega todos os IAxis associados a um Axis
    ''' </summary>
    ''' <param name="axisToGet"></param>
    ''' <returns></returns>
    Public Function GetAxis(ByVal axisToGet As Axis) As IEnumerable(Of IAxis)
        Return New HashSet(Of IAxis)(axisTab(axisToGet).Item4, equalityComparer)
    End Function

    ''' <summary>
    ''' Função de debug para entrada
    ''' </summary>
    ''' <param name="ax"></param>
    Private Sub DebugAxis(ByVal ax As Axis)
        If axisTab(ax).Item1 <> 0 Then
            Console.Write(ax.GetName(ax.GetType(), ax))
            Console.Write(", Value:")
            Console.Write(axisTab(ax).Item1)
            Console.Write(", Click:")
            Console.Write(axisTab(ax).Item2)
            Console.Write(", Pressed:")
            Console.Write(axisTab(ax).Item3)
            Console.WriteLine("")
        End If
    End Sub

    ''' <summary>
    ''' Atualiza valor de algum Axis na tabela
    ''' </summary>
    ''' <param name="axisToChange"> Axis para atualizar </param>
    Private Sub changeValueAxisTab(ByVal axisToChange As Axis)
        Dim value As Single = GetAxisValue(axisToChange)
        If axisTab.ContainsKey(axisToChange) Then
            Dim element As (Single, Boolean, Boolean, HashSet(Of IAxis)) = axisTab(axisToChange)
            axisTab(axisToChange) = (value, value <> 0 AndAlso Not element.Item3, value <> 0 AndAlso (element.Item2 OrElse element.Item3), element.Item4)
        Else
            axisTab(axisToChange) = (value, value <> 0, value <> 0, New HashSet(Of IAxis)(equalityComparer))
        End If
        If DebugMode Then
            DebugAxis(axisToChange)
        End If
    End Sub

    ''' <summary>
    ''' Atualiza os valores do AxisTab
    ''' </summary>
    Private Sub resetAxisTab()
        changeValueAxisTab(Axis.Horizontal)
        changeValueAxisTab(Axis.Vertical)
        changeValueAxisTab(Axis.MouseX)
        changeValueAxisTab(Axis.MouseY)
        changeValueAxisTab(Axis.Fire0)
        changeValueAxisTab(Axis.Fire1)
        changeValueAxisTab(Axis.Fire2)
        changeValueAxisTab(Axis.Jump)
        changeValueAxisTab(Axis.Cancel)
        changeValueAxisTab(Axis.Submit)
        changeValueAxisTab(Axis.MouseScrollWheel)
        changeValueAxisTab(Axis.Start)
        changeValueAxisTab(Axis.Options)
    End Sub

    ''' <summary>
    ''' Pega valor de algum Axis
    ''' </summary>
    ''' <param name="axis"> Axis a pegar </param>
    ''' <returns> Valor do Axis </returns>
    Private Function GetAxisValue(ByVal axis As Axis) As Single
        Dim output As Single = 0F

        If axisTab.ContainsKey(axis) Then
            For Each element As IAxis In axisTab(axis).Item4
                If output = 0 Then
                    output = element.GetValue()
                Else
                    Dim preout As Single = element.GetValue()
                    If preout * preout > output * output Then
                        output = preout
                    End If
                End If
            Next
        End If

        Return output
    End Function

End Class

