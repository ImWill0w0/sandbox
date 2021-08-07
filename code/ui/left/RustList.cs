using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;

[Library]
public partial class RustList : Panel
{
	VirtualScrollPanel Canvas;

	public RustList()
	{
		AddClass( "spawnpage" );
		AddChild( out Canvas, "canvas" );

		Canvas.Layout.AutoColumns = true;
		Canvas.Layout.ItemSize = new Vector2( 100, 100 );
		Canvas.OnCreateCell = ( cell, data ) =>
		{
			var file = (string)data;
			var panel = cell.Add.Panel( "icon" );
			panel.AddEventListener( "onclick", () => ConsoleSystem.Run( "spawn", "models/" + file ) );
			panel.Style.Background = new PanelBackground
			{
				Texture = Texture.Load( $"/models/{file}_c.png", false )
			};
		};

		foreach ( var file in FileSystem.Mounted.FindFile( "models", "*.vmdl_c.png", true ) )
		{
			if ( string.IsNullOrWhiteSpace( file ) ) continue;
			if ( file.Contains( "_lod0" ) ) continue;
			if ( file.Contains( "clothes" ) ) continue;
			if (file.Contains("sbox_props")) continue;
			if (file.Contains("entity")) continue;
			if (file.Contains("citizen_props")) continue;

			Canvas.AddItem( file.Remove( file.Length - 6 ) );
		}
	}
}
