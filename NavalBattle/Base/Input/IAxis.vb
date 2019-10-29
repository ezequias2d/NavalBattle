#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' IAxis.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Interface de entrada de Axis
''' 
''' Implementadas por:
'''     AxisGamePad
'''     AxisKeyboard
'''     AxisMouse
''' </summary>
Public Interface IAxis
    ''' <summary>
    ''' Axis associado
    ''' </summary>
    ''' <returns> Axis </returns>
    Property Axis As Axis

    ''' <summary>
    ''' Calcula o valor do Axis.
    ''' </summary>
    ''' <returns> Valor do Axis. </returns>
    Function GetValue() As Single
End Interface
