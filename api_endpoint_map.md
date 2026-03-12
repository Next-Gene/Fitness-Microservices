# 📋 API Gateway Endpoint Map

**Gateway URL**: `http://localhost:8088`

All endpoints below go through the gateway. Add `Authorization: Bearer <token>` header where 🔒 is shown.

---

## 🔓 Authentication (No Token Needed)

| # | Method | URL |
|---|--------|-----|
| 1 | `POST` | `http://localhost:8088/api/auth/register` |
| 2 | `POST` | `http://localhost:8088/api/auth/login` |
| 3 | `POST` | `http://localhost:8088/api/auth/forget-password` |
| 4 | `POST` | `http://localhost:8088/api/auth/verify-otp` |
| 5 | `POST` | `http://localhost:8088/api/auth/reset-password` |

## 🔒 Authentication (Token Required)

| # | Method | URL |
|---|--------|-----|
| 6 | `GET`  | `http://localhost:8088/api/auth/user-info` |
| 7 | `POST` | `http://localhost:8088/api/auth/change-password` |
| 8 | `PUT`  | `http://localhost:8088/api/auth/update-profile` |
| 9 | `POST` | `http://localhost:8088/api/auth/logout` |

---

## 🏋️ Workouts (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 10 | `GET`  | `http://localhost:8088/api/workouts` |
| 11 | `GET`  | `http://localhost:8088/api/workouts/{id}` |
| 12 | `GET`  | `http://localhost:8088/api/workouts/category/{categoryName}` |
| 13 | `POST` | `http://localhost:8088/api/workouts/{id}/start` |
| 14 | `POST` | `http://localhost:8088/api/workouts/create` |

## 📝 Workout Plans (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 15 | `GET`  | `http://localhost:8088/api/workout-plans` |
| 16 | `GET`  | `http://localhost:8088/api/workout-plans/{id}` |
| 17 | `POST` | `http://localhost:8088/api/workout-plans/{planId}/assign-to/{workoutId}` |

---

## 🥗 Nutrition (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 18 | `GET`  | `http://localhost:8088/api/nutrition/recommendations` |
| 19 | `GET`  | `http://localhost:8088/api/nutrition/meals/{id}` |

---

## 📊 Progress Tracking (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 20 | `GET`  | `http://localhost:8088/api/progress?userId={guid}&period=weekly` |
| 21 | `POST` | `http://localhost:8088/api/progress/workouts` |
| 22 | `POST` | `http://localhost:8088/api/progress/weight` |

---

## 🔢 Fitness Calculator (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 23 | `POST` | `http://localhost:8088/api/fitness-calculator` |
| 24 | `PUT`  | `http://localhost:8088/api/fitness-calculator/{userId}` |

---

## 🤖 Smart Coach (🔒 Token Required)

| # | Method | URL |
|---|--------|-----|
| 25 | `POST` | `http://localhost:8088/api/smart-coach` |

---

## 📦 Sample Request Bodies

### Register (POST /api/auth/register)
```json
{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@example.com",
  "password": "P@ssw0rd123",
  "phoneNumber": "01012345678",
  "height": 175,
  "weight": 80,
  "age": 25,
  "gender": "Male",
  "activtyLevel": "Moderate",
  "goal": "LoseWeight",
  "rememberMe": false
}
```

### Login (POST /api/auth/login)
```json
{
  "email": "ahmed@example.com",
  "password": "P@ssw0rd123",
  "rememberMe": true
}
```

### Smart Coach (POST /api/smart-coach)
```json
{
  "text": "What exercises should I do to lose belly fat?"
}
```
