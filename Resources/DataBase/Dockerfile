FROM mariadb:11.2.3
COPY ./Resources/DataBase/my.cnf /etc/mysql/conf.d/

ENV MYSQL_ROOT_PASSWORD=sgsRootPassword
ENV MYSQL_DATABASE=budget-bringer
ENV MYSQL_USER=sgsanalysisuser
ENV MYSQL_PASSWORD=analysisPassword

EXPOSE 3306