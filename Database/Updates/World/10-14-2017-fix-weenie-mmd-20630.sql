# Fix weenie correct burden for 1 mmd note
UPDATE ace_object_properties_int
SET propertyValue = 5
WHERE intPropertyId = 5 AND aceObjectId = 20630;

# Fix weenie correct value for 1 mmd note
UPDATE ace_object_properties_int
SET propertyValue = 250000
WHERE intPropertyId = 19 AND aceObjectId = 20630;

# Fix weenie correct stacksize for 1 mmd note
UPDATE ace_object_properties_int
SET propertyValue = 1
WHERE intPropertyId = 12 AND aceObjectId = 20630;