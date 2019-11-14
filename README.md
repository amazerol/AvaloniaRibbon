# Avalonia Ribbon
Ribbon Control for Avalonia

The purpose of this Avalonia component is to achieve something like this (the component can already be used but is definitely not considered as completed) :

![2019-06-16_18-00-19](https://user-images.githubusercontent.com/16206389/59566444-a752ea80-9060-11e9-828b-593d347f797d.gif)

Here is an example of syntax :
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



