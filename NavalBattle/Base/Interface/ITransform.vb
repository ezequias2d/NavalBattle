Imports Microsoft.Xna.Framework

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' ITransform.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Interface de transformador espacial(Posição, rotação e escala)
''' </summary>
Public Interface ITransform
    ''' <summary>
    ''' Posição no espaço virtual
    ''' </summary>
    ''' <returns> Posição </returns>
    Property Position As Vector2

    ''' <summary>
    ''' Angulo de rotação sobre o eixo Z
    ''' </summary>
    ''' <returns></returns>
    Property Angle As Single

    ''' <summary>
    ''' Escala no espaço virtual
    ''' </summary>
    ''' <returns> Escala no espaço virtual </returns>
    Property Scale As Vector2
End Interface
