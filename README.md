# Avalonia Ribbon
Ribbon Control for Avalonia

The purpose of this Avalonia component is to achieve something like Microsoft Ribbon UI found in Office and all around Windows operating system. The component can already be used but is definitely not considered as complete. I had to implement ribbon interface for my project **[Jaya - Cross Plat](https://github.com/waliarubal/Jaya)** so had to tailor things accordingly. 

The original piece of work is from [Alban Mazerolles](https://github.com/amazerol) which can be found [here](https://github.com/amazerol/AvaloniaRibbon).

Here is an example of the syntax :
```xaml
    <v:RibbonWindow>
      <v:RibbonControl>
        <v:RibbonTab Header="RibbonTab 1">
          <StackPanel Orientation="Horizontal">
            <v:RibbonTabGroup Text="Un premier groupe">
              <StackPanel Orientation="Horizontal">
                <v:RibbonButton Text="Test3" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
              </StackPanel>
            </v:RibbonTabGroup>
            <v:RibbonTabGroup Text="Un premier groupe">
              <StackPanel Orientation="Horizontal">
                <v:RibbonButton Text="Test3" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
                <v:RibbonButton Text="Test4" IconPath="settings.png" />
              </StackPanel>
            </v:RibbonTabGroup>
          </StackPanel>
        </v:RibbonTab>
        <v:RibbonTab Header="RibbonTab 2">qsdfqsdf</v:RibbonTab>
      </v:RibbonControl>
    </v:RibbonWindow>
```



