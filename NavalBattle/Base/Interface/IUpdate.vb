Imports Microsoft.Xna.Framework

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' IUpdate.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Interface com metodo de atualização a cada quadro do jogo.
''' </summary>
Public Interface IUpdate
    ''' <summary>
    ''' Determina se está ativado o Update
    ''' </summary>
    ''' <returns> Se update está ativado </returns>
    Property UpdateEnable As Boolean

    ''' <summary>
    ''' Função Update
    ''' </summary>
    ''' <param name="gameTime"> GameTime </param>
    Sub Update(ByVal gameTime As GameTime)
End Interface
