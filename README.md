# eGandalf.Epi.Validation
A series of attribute-based validators for Episerver Page, Block, and Media types.

Included validators by namespace:

## eGandalf.Epi.Validation.General

* *RequiredForPublish* - Requires a field, but at a different stage from `[Required]`. `[Required]` must be entered before a content instance is created, interrupting the author flow for some tasks (it's perfect for some others). This validator pushes validation to post-creation / pre-publish so authors can jump right into editing.

## eGandalf.Epi.Validation.Lists

Most of these should be self-explanatory.

* *ExactCount*
* *Minimum*
* *Maximum*
* *ExactCountOfType*
* *MinimumOfType*
* *MaximumOfType*

The "OfType" validators verify that a content area meets the constraints for a specific type of object. E.g. the StartPage must contain one Carousel > `[ExactCountOfType(1, typeof(CarouselBlock))]`. Allows multiple.

## eGandalf.Epi.Validation.Media

* *AllowedFileExtensions*
* *ExactDimensions*
* *MinimumDimensions*
* *MaximumDimensions*
* *MaximumFileSize*

The "Dimensions" attributes should be used for types that can be cast to ImageData.

## eGandalf.Epi.Validation.Text

* *DoesNotContain* - Ensures the given text is not present within the field. Case insensitive.
