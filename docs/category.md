# Get Category By Id

## Goal

Retrieve a category by its identifier.

---

## Request

### Endpoint

| Method | Route |
|--------|-------|
| GET | `/api/v1/categories/{id}` |

### Route Parameters

| Name | Type | Required |
|------|------|----------|
| id | Guid | ✅ |

---
## Main Flow

1. ⛔️ Authenticate the user.
2. ⛔️ Load the category by its identifier.
3. ⛔️ Map the category to the response.
4. ⛔️ Return `200 OK` with the category.
---

## Responses

| Status | Description | Progress |
|--------|-------------|----------|
| 200 OK | Category retrieved successfully. | ⛔️ |
| 401 Unauthorized | The user is not authenticated. | ⛔️ |
| 404 Not Found | The category does not exist. | ⛔️ |
| 500 Internal Server Error | An unexpected error occurred. | ⛔️ |

---

## Response

### 200 OK

```json
{
  "id": "6d9b7a0d-0b4b-4a88-b1e5-18c0ef9b0f2f",
  "name": "Electronics",
  "description": "Electronic devices and accessories",
  "createdAt": "2026-07-07T12:00:00Z",
  "updatedAt": "2026-07-07T12:00:00Z"
}
```

---
# Update Category

## Goal

Update an existing category.

## Request

### Endpoint

| Method | Route                     |
|--------|---------------------------|
| PUT | `/api/v1/categories/{id}` |

### Route Parameters

| Name | Type | Required |
|------|------|----------|
| id | Guid | ✅ |

### Body

| Name | Type | Required |
|------|------|---------|
| name | string | ✅ |
| description | string | ⛔️ |

---

## Main Flow

1. ⛔️ Authenticate the user.
2. ⛔️ Validate the request body.
3. ⛔️ Check if the category exists.
4. ⛔️ Check if the given category (if provided) exists.
5. ⛔️ Update only the provided fields.
7. ⛔️ Return `204 NotContent`.
---

## Responses

| Status                    | Description                                      | Progress |
|---------------------------|--------------------------------------------------|----------|
| 204 Not Content           | Category updated successfully.                   | ⛔️ |
| 400 Bad Request           | Invalid field value.                             | ⛔️ |
| 401 Unauthorized          | The user is not authenticated.                   | ⛔️ |
| 403 Forbidden             | The user is not authorized to update categories. | ⛔️ |
| 404 Not Found             | The category does not exist.                     | ⛔️ |
| 500 Internal Server Error | An unexpected error occurred.                    | ⛔️ |

