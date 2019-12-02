Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Content
Imports Microsoft.Xna.Framework.Graphics

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' AxisMouse.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Classe abstrata base de sena de jogo
''' </summary>
Public MustInherit Class GameScene
    ''' <summary>
    ''' Gerenciador de conteudo(usado para carregar recursos como textura e audio)
    ''' </summary>
    Protected content As ContentManager

    ''' <summary>
    ''' Camera padrão
    ''' </summary>
    ''' <returns> Camera </returns>
    Public Property Camera As Camera

    ''' <summary>
    ''' GUIController que controla da cena(Apenas leitura)
    ''' </summary>
    ''' <returns> Pega GUIController da cena </returns>
    Public ReadOnly Property GUIController As GUIController
        Get
            Return _GUIController
        End Get
    End Property

    ''' <summary>
    ''' Coleção de objetos para atualizar
    ''' </summary>
    Public ReadOnly Property Updates As ICollection(Of IUpdate)
        Get
            Return _updates
        End Get
    End Property

    Private _GUIController As GUIController

    Protected _updates As ICollection(Of IUpdate)

    ''' <summary>
    ''' Contrutor de GameScene
    ''' </summary>
    Public Sub New()
        Camera = New Camera(New Vector2(1024, 576) / 2.0F, ScreenManager.Instance.SpriteBatch)
        _GUIController = New GUIController(Camera.InternalDimensions)
    End Sub

    ''' <summary>
    ''' Função para carregar recursos(não gerenciados).
    ''' 
    ''' Se sobrescrita, deve ser chamada a função da classe base antes de qualquer codigo que envolva o content
    ''' ou GUIController.
    ''' </summary>
    Public Overridable Sub LoadContent()
        content = New ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content")
        Camera.Drawings.Add(GUIController)
    End Sub

    ''' <summary>
    ''' Função para descarregar recursos(não gerenciados).
    ''' </summary>
    Public Overridable Sub UnloadContent()
        content.Unload()
        Camera.Drawings.Clear()
        GUIController.MainContext.Clear()
        GUIController.ClearStack()
    End Sub

    ''' <summary>
    ''' Função principal de Update
    ''' 
    ''' Atualiza Input e GUIController e, em seguida, Chama update de todas os objetos em updates se element.UpdateEnable
    ''' </summary>
    ''' <param name="gameTime"></param>
    Public Overridable Sub Update(gameTime As GameTime)
        Input.Instance.Update(gameTime)
        GUIController.Update(gameTime)
        For Each element As IUpdate In updates
            If element.UpdateEnable Then
                element.Update(gameTime)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Desenha a cena
    ''' </summary>
    ''' <param name="spriteBatch"></param>
    Public Overridable Sub Draw(ByRef spriteBatch As SpriteBatch)
        Camera.Draw()
    End Sub

End Class
