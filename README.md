# CV-Handling-API

1. GetPersons
   [GET]
   http://localadress/persons
   to find all persons and related experience/education
   You can also add param: id for specific person
   example  http://localadress/persons?id=1

2. Post new person
   [POST]
   http://localadress/persons/new
   required fields are: "firstName", "lastName", "email", "phone"
   if these are not provided you will get error message: provide all fields
   these are added through query params so 
   http://localadress/persons/new?firstname=louis
   etc,
   on success you will get a link to the new person http://localadress/persons/2
   and created person as object.

3. find experience of specifik person
    [GET] 
  "/persons/{id}/work" where id= personid


4. add new experience
   [POST]
   "/persons/{id}/work/add" where id = personid
   required query keys are "Title", "Company", "StartDate"
   EndDate is optional will be set to null if not assigned
   Date format should be YYYY-MM-DD
   you can also set WorkDescription if you include that key.

5. update existing experience
   [PUT]
   "/persons/{pid}/work/{id}/update" where pid= personid and id= experienceid
   query params optional same as add new experience but if Only Title is added to params only Title will be changed.
   if you set optional params to empty for example "EndDate:" it will remove current enddate and set it to null.

6. Delete Existing experience
   [DELETE]
   "/persons/{pid}/work/{id}/remove" where pid= personid and id= experienceid

Educations work largely the same as experience

7. Find existing educations
   [GET]
   "/persons/{id}/educations"

8.add new education
  [POST]
  "/persons/{id}/educations/new"
  required params :  "School", "Degree", "StartDate", "EndDate" to create education
  Date format : YYYY-MM-DD
  EduDescription is optional field if you want to add description

9. Update existing Education
    [PUT]
   "/persons/{pid}/educations/{id}/update"
   all fields optional will update depending on input, if Only input params are Degree and School only these fields will be changed.

10. Delete education
    [DELETE]
    "/persons/{pid}/educations/{id}/remove"
    PersonID= pid, educationid= id, if these 2 ids are found education will be deleted
   
  

   
