FROM mariadb:10.5
COPY ./Resources/DataBase/my.cnf /etc/mysql/conf.d/

ENV MYSQL_ROOT_PASSWORD=sgsRootPassword
ENV MYSQL_DATABASE=budget-bringer
ENV MYSQL_USER=sgsanalysisuser
ENV MYSQL_PASSWORD=analysisPassword

EXPOSE 3306