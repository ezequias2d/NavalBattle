Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AxisKeyboard.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' AxisKeyboard
''' Eixo que varia de -1.0F a 1.0F utilizando como entrada o teclado
''' </summary>
Public Class AxisKeyboard
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
    Public Property Positive As Keys

    ''' <summary>
    ''' Botão que altera negativamente
    ''' </summary>
    ''' <returns> Botão associado negativamente </returns>
    Public Property Negative As Keys

    ''' <summary>
    ''' Construtor de AxisKeyboard
    ''' </summary>
    ''' <param name="axis"> Axis associado </param>
    ''' <param name="positive"> Botão que altera positivamente </param>
    ''' <param name="negative"> Botão que altera negativamente </param>
    Sub New(ByVal axis As Axis, ByVal positive As Keys, ByVal negative As Keys)
        Me.Axis = axis
        Me.Positive = positive
        Me.Negative = negative
    End Sub

    ''' <summary>
    ''' Calcula o valor do Axis.
    ''' </summary>
    ''' <returns> Valor do Axis. </returns>
    Public Function GetValue() As Single Implements IAxis.GetValue
        Dim state As KeyboardState = Keyboard.GetState()

        Dim output As Single = 0

        output += 1.0F * (state.IsKeyDown(Positive))

        output -= 1.0F * (state.IsKeyDown(Negative))

        Return output
    End Function
End Class
