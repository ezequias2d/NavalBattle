Imports Microsoft.Xna.Framework.Graphics

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' IDrawable.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Interface para algo desenhavel pela Camera
''' </summary>
Public Interface IDrawable
    ''' <summary>
    ''' Determina se está ativado para desenhar.
    ''' Se verdadeiro: desenha
    ''' Se falso: não desenha
    ''' </summary>
    ''' <returns> Se ativado o desenho </returns>
    Property DrawEnable As Boolean

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Property Layer As Long

    ''' <summary>
    ''' Função de desenho
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    Sub Draw(ByRef spriteBatch As SpriteBatch)
End Interface
