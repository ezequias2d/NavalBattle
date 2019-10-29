Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AxisMouse.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' AxisMouse
''' Eixo que varia de -1.0F a 1.0F utilizando como entrada o mouse
''' </summary>
Public Class AxisMouse
    Implements IAxis

    ''' <summary>
    ''' Axis associado
    ''' </summary>
    ''' <returns> Axis </returns>
    Public Property Axis As Axis Implements IAxis.Axis

    ''' <summary>
    ''' Botão que altera positivamente
    ''' </summary>
    ''' <returns> Botão associado positivamente </returns>
    Public Property Positive As MouseButton

    ''' <summary>
    ''' Botão que altera negativamente
    ''' </summary>
    ''' <returns> Botão associado negativamente </returns>
    Public Property Negative As MouseButton

    Private Shared notInitializedPositions As Boolean
    Private Shared oldPosition As Vector2

    Public Shared Property CenterMouse As Boolean

    ''' <summary>
    ''' Construtor de AxisMouse
    ''' </summary>
    ''' <param name="axis"> Axis </param>
    ''' <param name="positive"> Botão positivo </param>
    ''' <param name="negative"> Botão negativo </param>
    Public Sub New(axis As Axis, positive As MouseButton, negative As MouseButton)
        Me.Axis = axis
        Me.Positive = positive
        Me.Negative = negative
        CenterMouse = False
        ResetMousePosition()
    End Sub

    ''' <summary>
    ''' Reseta posição do mouse para centro da tela se flag CenterMouse e o eixo possui o botão X ou Y no negativo ou positivo
    ''' </summary>
    Private Sub ResetMousePosition()
        If CenterMouse AndAlso (Positive = MouseButton.X OrElse Negative = MouseButton.X OrElse Positive = MouseButton.Y OrElse Negative = MouseButton.Y) Then
            oldPosition = New Vector2(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight) / 2.0F
            Mouse.SetPosition(oldPosition.X, oldPosition.Y)
        End If
    End Sub

    ''' <summary>
    ''' Calcula o valor do Axis.
    ''' </summary>
    ''' <returns> Valor do Axis. </returns>
    Public Function GetValue() As Single Implements IAxis.GetValue
        Dim output As Single = 0F
        Dim state As MouseState = Mouse.GetState()
        ResetMousePosition()
        Select Case Positive
            Case MouseButton.Left
                output += 1.0F * (state.LeftButton = ButtonState.Pressed)
            Case MouseButton.Right
                output += 1.0F * (state.RightButton = ButtonState.Pressed)
            Case MouseButton.Middle
                output += 1.0F * (state.RightButton = ButtonState.Pressed)
            Case MouseButton.X
                If notInitializedPositions Then
                    notInitializedPositions = True
                    oldPosition = New Vector2(state.X, state.Y)
                Else
                    output += (state.X - oldPosition.X) / (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + 0F)
                    oldPosition.X = state.X
                End If
            Case MouseButton.Y
                If notInitializedPositions Then
                    notInitializedPositions = True
                    oldPosition = New Vector2(state.X, state.Y)
                Else
                    output += (state.Y - oldPosition.Y) / (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + 0F)
                    oldPosition.Y = state.Y
                End If
        End Select

        Select Case Negative
            Case MouseButton.Left
                output -= 1.0F * (state.LeftButton = ButtonState.Pressed)
            Case MouseButton.Right
                output -= 1.0F * (state.RightButton = ButtonState.Pressed)
            Case MouseButton.Middle
                output -= 1.0F * (state.RightButton = ButtonState.Pressed)
            Case MouseButton.X
                If notInitializedPositions Then
                    notInitializedPositions = True
                    oldPosition = New Vector2(state.X, state.Y)
                Else
                    output -= (state.X - oldPosition.X) / (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + 0F)
                    oldPosition.X = state.X
                End If
            Case MouseButton.Y
                If notInitializedPositions Then
                    notInitializedPositions = True
                    oldPosition = New Vector2(state.X, state.Y)
                Else
                    output -= (state.Y - oldPosition.Y) / (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + 0F)
                    oldPosition.Y = state.Y
                End If
        End Select

        Return output
    End Function
End Class
