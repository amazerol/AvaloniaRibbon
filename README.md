# AvaloniaRibbon
Ribbon for avalonia

The purpose of this Avalonia component is to achieve something like this (the component can already be used but is definitely not considered as completed) :

![2019-06-16_18-00-19](https://user-images.githubusercontent.com/16206389/59566444-a752ea80-9060-11e9-828b-593d347f797d.gif)

By downloading this project, you should be able to obtain the same result (don't use Avalonia 0.8.0 but the nightly build).

# Update (16/06/2019)
- Improvement of the look&feel of the ribbon
- Update of the previous gif with new look&feel

The remaining things to do :
- Take care of the disapearance of icons in case of window resizing
- Handle easily click actions

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


# Update (17/06/2019)
- Control fully usable
- NuGet done
- Wiki written

The remaining things to do :
- implements more controls (especially TOGGLE button)
- allow color stylesheets for a fully customizable control
- take care of the resizing matter (I don't know how to do yet ...)
