FROM envoyproxy/envoy:v1.7.0
COPY config/bootstrap.yaml /etc/envoy/envoy.yaml
CMD /usr/local/bin/envoy -c /etc/envoy/envoy.yaml --service-cluster envoy1 -l trace