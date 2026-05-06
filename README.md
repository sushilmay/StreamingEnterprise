# StreamingEnterprise

StreamingEnterprise is a .NET solution that demonstrates enterprise-style micro service with onion architecture with modular layers, containerized deployment, and integrated observability using Prometheus, Grafana, and Kibana.

---

## 📂 Project Structure
<img width="528" height="277" alt="image" src="https://github.com/user-attachments/assets/45691250-bf83-4031-8c06-501ff6b42a12" />

---

## 🛠️ Tech Stack
- **Language & Framework**: C#, .NET 6 / ASP.NET Core
- **Architecture**: Layered (Application, Domain, Infrastructure, Services)
- **Containerization**: Docker & Docker Compose
- **Monitoring & Metrics**: Prometheus, Grafana
- **Logging & Visualization**: ElasticSearch, Kibana

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 6+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [Prometheus](https://prometheus.io/)
- [Grafana](https://grafana.com/)
- [ElasticSearch & Kibana](https://www.elastic.co/)

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/sushilmay/StreamingEnterprise.git
   cd StreamingEnterprise
2. Build and run with Docker:
   docker-compose up --build
3. Access monitoring tools:
   Prometheus: http://localhost:9090
   Grafana: http://localhost:3000
   Kibana: http://localhost:5601
4. Documentation
   HLD.docx contains the high-level design overview.
   Each service exposes Swagger/OpenAPI documentation at /swagger when running locally.
   Grafana dashboards (dashboard-*.json) provide visual metrics.
