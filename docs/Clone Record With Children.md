Clones a record together with its child records in one step: the copy of each child belongs to the copy of the record, not to the original. It does what Clone Record followed by Clone Children would do, without having to pass the new record's URL from one step to the next. Use it, for example, to copy a project with its tasks or an account with its contacts.

### Inputs

* **Source Record URL (required)** : the URL of the record to clone (use the Record URL (Dynamic) field of the record).
* **Relationship Names (required)** : the schema names of the one-to-many relationships from the record to the children to clone, separated by "," or ";", for example `account_primary_contact` or `new_project_tasks; new_project_milestones`. Each relationship must start at the source record's table; if one doesn't, the step fails before anything is copied.
* **Prefix (optional)** : text added in front of the name of the record's copy.
* **Fields to Ignore (optional)** : the record's columns not to copy, separated by ";" or "," (in lowercase).
* **Child Prefix (optional)** : text added in front of the name of each child's copy.
* **Child Fields to Ignore (optional)** : the children's columns not to copy, separated by ";" or "," (in lowercase).
* **Copy Child Status (optional, default No)** : Yes gives each child's copy the status and status reason of the child it was copied from, so an inactive child gives an inactive copy. No: every child's copy starts in its default (active) state.
* **Only Active Children (optional, default No)** : Yes only copies the active children (status Active). The child tables must have a status; tables without one, such as quote and order lines, can't use this option.

### Outputs

* **Cloned Guid** : the GUID of the record's copy, as text.
* **Children Cloned** : the number of child records copied, over all the relationships.

### Notes

* The record's copy always starts active, as with Clone Record.
* Each child's copy gets the lookup of its relationship set to the record's copy when it is created, so the original record and its children are never changed.
* Only the direct children are copied, not their own children.
* Copy Child Status sets an inactive status after the copy is created, the way you would change it by hand. Tables that are closed with their own message instead (for example a won opportunity or a resolved case) can't be given that status this way, and the step fails for them.
